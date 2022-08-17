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



        public virtual UnityEngine.Color Draw(Cell cell)
        {
            //Displays the path to the exit, monster or item in a dark green color
            if (cell.IsOnBestPath)
                return new(0f, .75f, 0f, 1f);
            else
                return new(.25f, .25f, .25f, 1f);
        }
    }
}