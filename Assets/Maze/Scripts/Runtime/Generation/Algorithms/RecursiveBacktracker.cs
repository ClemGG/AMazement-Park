using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class RecursiveBacktracker : IGeneration
    {
        public GenerationProgressReport Report { get; set; } = new();

        public void ExecuteSync(IGrid grid, Cell startCell = null)
        {
            Stack<Cell> stack = new();
            Cell start = startCell ?? grid.RandomCell();
            stack.Push(start);

            while(stack.Count > 0)
            {
                Cell current = stack.Peek();
                List<Cell> neighbors = new();
                for (int i = 0; i < current.Neighbors.Count; i++)
                {
                    Cell unvNeighbor = current.Neighbors[i];
                    if (unvNeighbor.Links.Count == 0)
                    {
                        neighbors.Add(unvNeighbor);
                    }
                }

                if(neighbors.Count == 0)
                {
                    stack.Pop();
                }
                else
                {
                    Cell neighbor = neighbors.Sample();
                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }
        }



        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            
            List<Cell> linkedCells = new();

            Stack<Cell> stack = new();
            start ??= grid.RandomCell();
            stack.Push(start);

            while (stack.Count > 0)
            {
                Cell current = stack.Peek();
                List<Cell> neighbors = new();
                for (int i = 0; i < current.Neighbors.Count; i++)
                {
                    Cell unvNeighbor = current.Neighbors[i];
                    if (unvNeighbor.Links.Count == 0)
                    {
                        neighbors.Add(unvNeighbor);
                    }
                }

                if (neighbors.Count == 0)
                {
                    stack.Pop();
                }
                else
                {
                    Cell neighbor = neighbors.Sample();
                    current.Link(neighbor);
                    stack.Push(neighbor);
                    linkedCells.Add(current);
                }



                Report.ProgressPercentage = (float)(linkedCells.Count * 100 / grid.Size()) / 100f;
                Report.UpdateTrackTime(Time.deltaTime);
                progress.Report(Report);
                yield return null;
            }
        }
    }
}