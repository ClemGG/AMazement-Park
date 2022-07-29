using Project.Models.Maze;
using Project.Procedural.MazeGeneration;
using UnityEngine;
using Project.Models.Game.Enums;

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


        private void OnApplicationQuit()
        {
            Cleanup();
        }

        //In the Game scene, automatically loads the level
        [ContextMenu("Execute Generation Algorithm On Start")]
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
            Execute();
        }


        [ContextMenu("Cleanup")]
        public void Cleanup()
        {
            if (DrawMethod is not null)
            {
                DrawMethod.Cleanup();
            }
        }


        [ContextMenu("Execute Generation Algorithm")]
        public void Execute()
        {
            SetupGrid();
            Generate();
            Draw();

        }


        private void SetupGrid()
        {
            Grid = ShowLongestPaths ? new ColoredGameGrid(Settings) : new GameGrid(Settings);
        }

        private void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Execute(Grid);

            //TODO : Change this to start distances at Player position
            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());

        }

        private void Draw()
        {
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);
            DrawMethod.Draw(Grid);
        }
    }
}