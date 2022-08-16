using System.Collections.Generic;
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
        public static CellHolder[][] CellsSlots { get; set; }

        #region Init

        public static void Init(IGrid grid)
        {
            Grid = grid;

            CellsSlots = new CellHolder[grid.Rows][];
            for (int i = 0; i < grid.Rows; i++)
            {
                CellsSlots[i] = new CellHolder[grid.Columns];

                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell c = grid[i, j];
                    if (c != null)
                    {
                        CellsSlots[i][j] = new(c);
                    }
                }
            }
        }

        public static void Cleanup()
        {
            Grid = null;
            CellsSlots = null;
        }

        #endregion


        #region Cell Entities

        //These methods will allow us to move the items and characters
        //at different locations and display their position accurately
        //on the map.
        public static CellHolder GetCellHolder(Cell c)
        {
            return CellsSlots[c.Row][c.Column];
        }
        public static CellHolder GetCellHolder(int i, int j)
        {
            return CellsSlots[i][j];
        }

        public static void AddEntityToCell(Cell c, IEntity e)
        {
            CellHolder ch = GetCellHolder(c);
            ch.Cell = c;
            ch.ObjectsOnThisCell.Add(e);
        }

        public static void RemoveEntityFromCell(Cell c, IEntity e)
        {
            GetCellHolder(c).ObjectsOnThisCell.Remove(e);
        }

        public static bool IsCellOccupied(Cell c)
        {
            return GetCellHolder(c).Occupied;
        }

        public static IEnumerable<Cell> GetAllOccupiedCells()
        {
            foreach (Cell cell in Grid.EachCell())
            {
                CellHolder ch = GetCellHolder(cell); 
                if (ch.Occupied)
                {
                    yield return ch.Cell;
                }
            }
        }

        #endregion

    }
}