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
        public static CellHolder[] AllOccupableCells { get; set; }

        #region Init

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
                    Cell c = grid[i, j];
                    if (c is not null)
                    {
                        AllOccupableCells[i * j] = new(c);
                    }
                }
            }
        }

        public static void Dispose()
        {
            Grid = null;
            AllOccupableCells = null;
        }

        #endregion


        #region Cell Entities

        public static CellHolder GetCellHolder(Cell c)
        {
            return AllOccupableCells[c.Row * c.Column];
        }
        public static CellHolder GetCellHolder(int i, int j)
        {
            return AllOccupableCells[i * j];
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
            for (int i = 0; i < AllOccupableCells.Length; i++)
            {
                CellHolder ch = AllOccupableCells[i];
                if (ch is not null && ch.Occupied)
                {
                    yield return ch.Cell;
                }
            }
        }

        #endregion
    }
}