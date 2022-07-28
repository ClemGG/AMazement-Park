
using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{

    //The Houston algorithm is a completely random algorithm, just like the Aldous Broder and the Wilson ones.
    //It combines the two for the best performance, starting with AB until some minimum number of cells
    //have been visited, and then switches to Wilson's.
    public class Houston : IGeneration
    {
        private readonly float _houstonSwapPercent = .5f;

        public Houston(GenerationSettingsSO settings)
        {
            _houstonSwapPercent = settings.HoustonSwapPercent;
        }

        public void Execute(IGrid grid, Cell start = null)
        {
            List<Cell> unvisited = new(grid.Size());
            foreach (Cell c in grid.EachCell())
            {
                if (c is null) continue;

                unvisited.Add(c);
            }

            Cell current = start ?? unvisited.Sample();
            unvisited.Remove(current);

            while (unvisited.Count > 0)
            {
                if(unvisited.Count / grid.Size() - 1 > _houstonSwapPercent)
                {
                    //Aldous-Broder
                    Cell neighbor = current;
                    if (neighbor.Links.Count == 0)
                    {
                        current.Link(neighbor);
                    }

                    unvisited.Remove(current);
                    current = neighbor;
                }
                else
                {
                    //Houston
                    Cell cell;
                    do
                    {
                        cell = unvisited.Sample();
                    }
                    while (cell is null);

                    List<Cell> path = new() { cell };

                    while (unvisited.Contains(cell))
                    {
                        cell = cell.Neighbors.Sample();
                        int position = path.IndexOf(cell);

                        if (position == -1)
                        {
                            path.Add(cell);
                        }
                        else
                        {
                            path = path.GetRange(0, position + 1);
                        }
                    }

                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        path[i].Link(path[i + 1]);
                        unvisited.Remove(path[i]);
                    }
                }

                
            }

        }
    }
}