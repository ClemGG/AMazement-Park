using System.Collections.Generic;
using Project.Models.Game;
using Project.Procedural.MazeGeneration;

namespace Project.Models.Maze
{

    //Used in the GameSession to keep track of the position
    //of each Entity in the maze
    public class CellHolder
    {
        public Cell Cell { get; set; }
        public List<IEntity> ObjectsOnThisCell { get; set; } = new();
        public bool Occupied { get => ObjectsOnThisCell.Count > 0; }

        public CellHolder(Cell c)
        {
            Cell = c;
        }
    }
}