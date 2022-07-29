using Project.Procedural.MazeGeneration;

namespace Project.Models.Maze
{
    public class GameGrid : Grid, IDrawableGrid, IDrawableGrid<UnityEngine.Color>
    {
        public GameGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public GameGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }

        public UnityEngine.Color Draw(Cell cell)
        {
            return UnityEngine.Color.black;
        }
    }
}