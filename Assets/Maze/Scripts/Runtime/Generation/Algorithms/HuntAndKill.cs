using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //The Hunt-And-Kill algorithm functions similarily to the A-B and Wilson's algorithms,
    //excepts when the current cell is in a corner with no unvisited Cells,
    //the algo goes into Hunt mode, where it will scan each row from the top,
    //find the first unvisited cell next to a visited one
    //and restart the process from that point after linking it
    //to a previously visited neighboring cell.
    public class HuntAndKill : IGeneration
    {
        public GenerationProgressReport Report { get; set; } = new();

        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            Cell current = start ?? grid.RandomCell();

            while(current is not null)
            {
                List<Cell> unvisitedCells = new();
                for (int i = 0; i < current.Neighbors.Count; i++)
                {
                    Cell unvNeighbor = current.Neighbors[i];
                    if(unvNeighbor.Links.Count == 0)
                    {
                        unvisitedCells.Add(unvNeighbor);
                    }
                }

                if(unvisitedCells.Count > 0)
                {
                    Cell neighbor = unvisitedCells.Sample();
                    current.Link(neighbor);
                    current = neighbor;
                }
                else
                {
                    current = null;

                    foreach (Cell cell in grid.EachCell())
                    {
                        if (cell is null) continue;

                        List<Cell> visitedCells = new();
                        for (int i = 0; i < cell.Neighbors.Count; i++)
                        {
                            Cell vNeighbor = cell.Neighbors[i];
                            if (vNeighbor.Links.Count > 0)
                            {
                                visitedCells.Add(vNeighbor);
                            }
                        }

                        if (cell.Links.Count == 0 && visitedCells.Count > 0)
                        {
                            current = cell;
                            Cell neighbor = visitedCells.Sample();
                            current.Link(neighbor);
                            break;
                        }
                    }
                }

               
            }
        }


        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            
            List<Cell> linkedCells = new();

            Cell current = start ?? grid.RandomCell();

            while (current is not null)
            {
                List<Cell> unvisitedCells = new();
                for (int i = 0; i < current.Neighbors.Count; i++)
                {
                    Cell unvNeighbor = current.Neighbors[i];
                    if (unvNeighbor.Links.Count == 0)
                    {
                        unvisitedCells.Add(unvNeighbor);
                    }
                }

                if (unvisitedCells.Count > 0)
                {
                    Cell neighbor = unvisitedCells.Sample();
                    current.Link(neighbor);
                    current = neighbor;

                    linkedCells.Add(current);
                }
                else
                {
                    current = null;

                    foreach (Cell cell in grid.EachCell())
                    {
                        if (cell is null) continue;

                        List<Cell> visitedCells = new();
                        for (int i = 0; i < cell.Neighbors.Count; i++)
                        {
                            Cell vNeighbor = cell.Neighbors[i];
                            if (vNeighbor.Links.Count > 0)
                            {
                                visitedCells.Add(vNeighbor);
                            }
                        }

                        if (cell.Links.Count == 0 && visitedCells.Count > 0)
                        {
                            current = cell;
                            Cell neighbor = visitedCells.Sample();
                            current.Link(neighbor);
                            linkedCells.Add(current);
                            break;
                        }
                    }
                }



                Report.ProgressPercentage = (float)(linkedCells.Count * 100 / grid.Size()) / 100f;
                Report.UpdateTrackTime(Time.deltaTime);
                progress.Report(Report);
                yield return null;
            }
        }
    }
}