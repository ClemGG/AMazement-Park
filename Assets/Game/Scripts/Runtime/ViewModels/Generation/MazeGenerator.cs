using Project.Models.Maze;
using Project.Procedural.MazeGeneration;
using UnityEngine;
using Project.Models.Game.Enums;
using System.Collections;
using System;
using static Project.Services.StringFormatterService;
using static Project.Procedural.MazeGeneration.Distances;

namespace Project.ViewModels.Generation
{
    //I don't use IDemo because I don't want to redraw the SO in the Inspector
    public class MazeGenerator : MonoBehaviour
    {
        [field: SerializeField] public bool GenerateOnStart { get; set; } = false;
        [field: SerializeField] public bool ShowLongestPaths { get; set; } = false;
        [field: SerializeField, ReadOnly] public CustomMazeSettingsSO Settings { get; set; }
        public IDrawableGrid Grid { get; set; }
        public IDrawMethod DrawMethod { get; set; }


        //This class will draw the maze asynchronously.
        //As the maze gets bigger, the game might freeze for a long time.
        //This allows us to mitigate this issue and display the progress on screen.
        private Progress<GenerationProgressReport> Progress { get; set; }
        private ProgressVisualizer ProgressVisualizer { get; set; } = new();


        //In the Game scene, automatically loads the level
        private void Start()
        {
            if (GenerateOnStart)
            {
                Execute(GameSession.DifficultyLevel, DrawMode.Mesh);
            }
        }

        //In the 2D scene, generates the level when the player presses the "Generate Maze" button
        public void Execute(Difficulty difficulty, DrawMode drawMode) 
        {
            Settings = Resources.Load<CustomMazeSettingsSO>($"Settings/{difficulty}");
            Settings.DrawMode = drawMode;
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

                Grid = ShowLongestPaths ? new MaskedColoredGameGrid(m) : new MaskedGameGrid(m);
            }
            else
            {
                Grid = ShowLongestPaths ? new ColoredGameGrid(Settings) : new GameGrid(Settings);
            }
        }

        public IEnumerator GenerateAsync()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Report = new(genAlg.GetType().Name.ToString().AddSpaces());

            Progress = new();
            Progress.ProgressChanged += OnGenerationProgressChanged;
            yield return StartCoroutine(genAlg.ExecuteAsync(Grid, Progress));
            Grid.Braid();

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());

        }


        public void DrawAsync()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);
            DrawMethod.Report = new("Rendering");

            Progress = new();
            Progress.ProgressChanged += OnDrawProgressChanged;
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
            Progress.ProgressChanged -= OnDrawProgressChanged;
        }
        private void OnGenerationProgressDone()
        {
            Progress.ProgressChanged -= OnGenerationProgressChanged;
        }
    }
}