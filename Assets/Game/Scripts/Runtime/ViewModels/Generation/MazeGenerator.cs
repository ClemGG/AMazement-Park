using Project.Models.Maze;
using Project.Procedural.MazeGeneration;
using UnityEngine;

namespace Project.ViewModels.Generation
{
    public class MazeGenerator : MonoBehaviour
    {
        [field: SerializeField] public bool GenerateOnStart { get; set; } = false;
        [field: SerializeField] public CustomMazeSettingsSO Settings { get; set; }
        public IDrawableGrid Grid { get; set; }
        public IDrawMethod DrawMethod { get; set; }


        private void OnApplicationQuit()
        {
            Cleanup();
        }

        private void Start()
        {
            if (GenerateOnStart)
            {
                Settings = Resources.Load<CustomMazeSettingsSO>("Settings/Custom");
                Settings.DrawMode = DrawMode.Mesh;
                Execute();
            }
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
            Grid = new ColoredGrid(Settings);
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