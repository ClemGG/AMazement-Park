namespace Project.Procedural.MazeGeneration
{
    public class AldousBroderDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            AldousBroder algorithm = new();
            algorithm.Execute(Grid);

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());
        }
    }
}