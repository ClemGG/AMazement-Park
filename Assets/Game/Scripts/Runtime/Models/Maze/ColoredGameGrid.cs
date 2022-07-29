using Project.Procedural.MazeGeneration;

namespace Project.Models.Maze
{
    public class ColoredGameGrid : Grid, IDrawableGrid, IDrawableGrid<UnityEngine.Color>
    {
        public ColoredGameGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public ColoredGameGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }

        public UnityEngine.Color Draw(Cell cell)
        {
            int distance = Distances[cell];
            float intensity = (float)(Maximum - distance) / Maximum;
            float dark = UnityEngine.Mathf.Round(255f * intensity);
            float bright = 128f + UnityEngine.Mathf.Round(127f * intensity);

            return new(bright / 255f, dark / 255f, dark / 255f, 1f);
        }
    }
}