using Project.Models.Maze;
using Project.Procedural.MazeGeneration;
using UnityEngine;
using Project.Models.Game.Enums;
using System.Collections;
using System;
using static Project.Services.StringFormatterService;
using static Project.Procedural.MazeGeneration.Distances;
using Project.Models.Game;
using System.Collections.Generic;
using Project.ViewModels.Maze;
using Project.Services;

namespace Project.ViewModels.Generation
{
    //I don't use IDemo because I don't want to redraw the SO in the Inspector
    public class MazeGenerator : MonoBehaviour
    {

        #region Fields

        [field: SerializeField] public bool GenerateOnStart { get; set; } = false;
        [field: SerializeField] public bool GenerateEntities { get; set; } = true;
        [field: SerializeField] public bool ShowBestPaths { get; set; } = false;
        [field: SerializeField] public bool ShowHeatMap { get; set; } = false;
        [field: SerializeField, ReadOnly] public CustomMazeSettingsSO Settings { get; set; }
        public IDrawableGrid Grid { get; set; }
        public IDrawMethod DrawMethod { get; set; }

        //This class will draw the maze asynchronously.
        //As the maze gets bigger, the game might freeze for a long time.
        //This allows us to mitigate this issue and display the progress on screen.
        private Progress<GenerationProgressReport> Progress { get; set; }
        private ProgressVisualizer ProgressVisualizer { get; set; } = new();
        private List<Cell> _occupiedCells = new(9); //used to combine the paths
        
        //Used by other scripts to register events when the maze is drawn in DrawAsync
        public event EventHandler<GenerationProgressReport> OnMazeDone;



        #endregion


        #region Mono

        //In the Game scene, automatically loads the level
        private void Awake()
        {
            if (GenerateOnStart)
            {
                Execute(GameSession.DifficultyLevel, DrawMode.Mesh);
            }
        }

        //In the 2D scene, generates the level when the player presses the "Generate Maze" button
        public void Execute(Difficulty difficulty, DrawMode drawMode) 
        {
            Settings = MazeSettingsService.SetupSettings(difficulty);
            Settings.DrawMode = drawMode;
            GameSession.Settings = Settings;
            ExecuteAsync();
        }

        public void Cleanup()
        {
            if (DrawMethod is not null)
            {
                DrawMethod.Cleanup();
            }

            StopAllCoroutines();
            ProgressVisualizer.Cleanup();
            OnProgressDone();
        }


        public void ExecuteAsync()
        {
            StopAllCoroutines();
            StartCoroutine(ExecuteAsyncCo());
        }

        private IEnumerator ExecuteAsyncCo()
        {
            SetupGrid();
            yield return GenerateAsync();
            DrawAsync();
        }

        #endregion



        #region Generate

        private void SetupGrid()
        {
            if (Settings.MaskName != "")
            {
                Mask m;
                switch (Settings.Extension)
                {
                    case ".txt":
                        m = Mask.FromText(Settings.MaskName);
                        break;
                    default:
                        m = Mask.FromImgFile(Settings.MaskName, Settings.Extension);
                        break;

                }

                Grid = ShowHeatMap ? new MaskedColoredGameGrid(m) : new MaskedGameGrid(m);
            }
            else
            {
                Grid = ShowHeatMap ? new ColoredGameGrid(Settings) : new GameGrid(Settings);
            }
        }

        public IEnumerator GenerateAsync()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Report = new(genAlg.GetType().Name.ToString().AddSpaces());

            Progress = new();
            Progress.ProgressChanged += OnGenerationProgressChanged;
            yield return StartCoroutine(genAlg.ExecuteAsync(Grid, Progress));
            Grid.Braid(Settings.BraidRate);


            //Displays the entities in the maze
            MazeManager.Init(Grid);
            if (GenerateEntities)
            {
                Cell start = Grid.RandomCell();
                _occupiedCells.Clear();
                AddMonstersAndItemsToGrid(start);
                Grid.SetDistances(GetDistancesOfAllEntities(start));
            }
        }

        private void AddMonstersAndItemsToGrid(Cell start)
        {

            var entities = Resources.LoadAll("Entities");
            Character[] characters = new Character[6];
            Item[] items = new Item[4];
            for (int i = 0; i < 6; i++)
            {
                characters[i] = entities[i] as Character;
            }
            for (int i = 6; i < 10; i++)
            {
                items[i-6] = entities[i] as Item;
            }

            //Get all Cells at least [GridSize /2] Cells away from the Player
            List<Cell> farthestCells = new();
            start.GetAllCellsBeyondDistance(Grid,
                                            Mathf.Min(Grid.Rows, Grid.Columns) / 2,
                                            farthestCells);
            


            //Add player
            IEntity player = characters[0];
            MazeManager.AddEntityToCell(start, player);
            _occupiedCells.Add(start);


            //Add exit
            IEntity exit = items[^1];

            Cell randomCell = farthestCells.Sample();
            MazeManager.AddEntityToCell(randomCell, exit);
            farthestCells.Remove(randomCell);
            _occupiedCells.Add(randomCell);

            //Add monsters
            for (int i = 1; i < characters.Length; i++)
            {
                //On doit faire la diff�rence entre les modes
                //car le GameSession.ActivityLevels est mis � 0 quand on g�n�re un nouveau d�dale,
                //ce qui emp�che les icones d'appara�tre en mode Custom
                bool shouldAddMonster =
                    GameSession.ActivityLevels[i - 1] > 1 && GameSession.DifficultyLevel != Difficulty.Custom ||
                    Settings.ActivityLevels[i - 1] > 1 && GameSession.DifficultyLevel == Difficulty.Custom;
                
                if (shouldAddMonster)
                {
                    IEntity monster = characters[i];

                    randomCell = farthestCells.Sample();
                    MazeManager.AddEntityToCell(randomCell, monster);
                    farthestCells.Remove(randomCell);
                    _occupiedCells.Add(randomCell);
                }
            }

            //Add items
            for (int i = 0; i < items.Length-1; i++)
            {
                //Chaque objet doit �tre instanci� pour fonctionner.
                //Ils seront d�truits automatiquement via leurs propres scripts
                //s'ils doivent �tre d�sactiv�s
                IEntity item = items[i];

                randomCell = farthestCells.Sample();
                MazeManager.AddEntityToCell(randomCell, item);
                farthestCells.Remove(randomCell);
                _occupiedCells.Add(randomCell);
            }
        }



        private Distances GetDistancesOfAllEntities(Cell start)
        {
            Distances distances = start.GetDistances();

            foreach (Cell cell in _occupiedCells)
            {
                if (cell != start)
                {
                    distances = Combine(distances, distances.PathTo(cell), ShowBestPaths);
                }
            }

            return distances;
        }

        #endregion



        #region Draw

        public void DrawAsync()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = GameInterfaceFactory.GetGameDrawMode(Settings);
            DrawMethod.Report = new("Rendering");

            Progress = new();
            Progress.ProgressChanged += OnDrawProgressChanged;
            Progress.ProgressChanged += OnMazeDone;
            StartCoroutine(DrawMethod.DrawAsync(Grid, Progress));
        }

        private void OnDrawProgressChanged(object sender, GenerationProgressReport e)
        {
            ProgressVisualizer.DisplayDrawProgress(e);
            if (Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                OnDrawProgressDone();
                ProgressVisualizer.HideCanvas();
            }
        }

        private void OnGenerationProgressChanged(object sender, GenerationProgressReport e)
        {
            ProgressVisualizer.DisplayGenerationProgress(e);
            if (Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                OnGenerationProgressDone();
            }
        }

        private void OnProgressDone()
        {
            OnDrawProgressDone();
            OnGenerationProgressDone();
        }
        private void OnDrawProgressDone()
        {
            if(Progress != null)
            {
                Progress.ProgressChanged -= OnDrawProgressChanged;
                Progress.ProgressChanged -= OnMazeDone;
            }
        }
        private void OnGenerationProgressDone()
        {
            if(Progress != null)
                Progress.ProgressChanged -= OnGenerationProgressChanged;
        }

        #endregion
    }
}