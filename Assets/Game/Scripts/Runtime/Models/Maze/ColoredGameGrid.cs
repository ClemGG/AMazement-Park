using Project.Procedural.MazeGeneration;
using UnityEngine;

namespace Project.Models.Maze
{
    public class ColoredGameGrid : GameGrid
    {
        public ColoredGameGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public ColoredGameGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }


        public override Color Draw(Cell cell)
        {
            int distance = Distances[cell];
            float intensity = (float)(Maximum - distance) / Maximum;
            
            //Draw only the paths leading to a monster or an item
            if (intensity > 1f) return base.Draw(cell);

            float dark = Mathf.Round(255f * intensity);
            float bright = 128f + Mathf.Round(127f * intensity);

            return new(bright / 255f, dark / 255f, dark / 255f, 1f);
        }
    }
}