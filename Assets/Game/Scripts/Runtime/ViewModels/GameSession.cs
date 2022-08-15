using Project.Models.Game;
using Project.Models.Game.Enums;
using Project.Models.Maze;
using Project.Procedural.MazeGeneration;

namespace Project.ViewModels
{
    public static class GameSession
    {
        public static Difficulty DifficultyLevel { get; set; } = Difficulty.Custom;
        public static IGrid Grid { get; set; }
        public static CellHolder[] AllOccupableCells { get; set; }
        
        public static void Init(IGrid grid)
        {
            Grid = grid;

            //The array will automatically size up correctly
            //even if we have a mask on the grid
            int size = grid.Size();
            AllOccupableCells = new CellHolder[size];
            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    AllOccupableCells[i*j] = new(grid[i, j]);
                }
            }
        }

        public static void AddEntityToCell(Cell c)
        {

        }

        public static void RemoveEntityFromCell(Cell c, IEntity e)
        {

        }


        public static void Dispose()
        {
            Grid = null;
            AllOccupableCells = null;
        }
    }
}