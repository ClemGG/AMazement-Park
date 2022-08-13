using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class Distances
    {
        public Cell Root { get; private set; }
        public Dictionary<Cell, int> Cells { get; private set; }

        public Distances(Cell root)
        {
            Root = root;
            Cells = new Dictionary<Cell, int>
            {
                { root, 0 }
            };
        }

        public int this[Cell cell]
        {
            get
            {
                if (Cells.ContainsKey(cell))
                {
                    return Cells[cell];
                }

                return -1;
            }
            set
            {
                Cells[cell] = value;
            }
        }

        public IEnumerable<Cell> GetAllCells()
        {
            foreach (Cell item in Cells.Keys)
            {
                yield return item;
            }
        }

        public Distances PathTo(Cell goal)
        {
            Cell current = goal;
            var breadcrumbs = new Distances(Root);
            breadcrumbs[current] = Cells[current];

            while(current != Root)
            {
                foreach (Cell neighbor in current.GetAllLinkedCells())
                {
                    //Retrives a cell with a distance number 1 unit lower
                    //than the current cell.
                    //This goes automatically from the goal to the start.
                    if(Cells[neighbor] < Cells[current])
                    {
                        breadcrumbs[neighbor] = Cells[neighbor];
                        current = neighbor;
                        break;
                    }
                }
            }



            return breadcrumbs;
        }

        //returns cell furthest to the root cell
        public (Cell cell, int dst) Max()
        {
            int maxDst = 0;
            Cell maxCell = Root;

            foreach (Cell cell in GetAllCells())
            {
                if(Cells[cell] > maxDst)
                {
                    maxCell = cell;
                    maxDst = Cells[cell];
                }
            }

            return (maxCell, maxDst);
        }


        //The boolean will allow us to draw the cells on a best path with a different color
        public static Distances Combine(Distances d1, Distances d2, bool setAsBestPath = false)
        {
            Distances combined = new(d1.Root);
            foreach (Cell cell in d1.GetAllCells())
            {
                combined.Cells[cell] = d1[cell];
            }
            foreach (Cell cell in d2.GetAllCells())
            {
                cell.IsOnBestPath = setAsBestPath;
                combined[cell] = d2[cell];
            }

            return combined;
        }
    }
}