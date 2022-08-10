using Project.Procedural.MazeGeneration;

namespace Project.Models.Maze
{
    public class MaskedGameGrid : GameGrid, IMaskable
    {
        public Mask Mask { get; set; }

        public MaskedGameGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public MaskedGameGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }



        public MaskedGameGrid(Mask mask) : base(mask.Rows, mask.Columns)
        {
            Mask = mask;
            PrepareGrid();
            ConfigureCells();
        }


        //We add "new" to these methods because we don't want to call the base
        //in order to use the mask
        public new void PrepareGrid()
        {
            _grid = new Cell[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                _grid[i] = new Cell[Columns];
                for (int j = 0; j < Columns; j++)
                {
                    if (Mask[i, j])
                    {
                        _grid[i][j] = new(i, j);
                    }
                    else
                    {
                        _grid[i][j] = null;
                    }
                }
            }
        }


        public new void ConfigureCells()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Cell c = _grid[i][j];

                    if (c is not null)
                    {
                        c.North = this[i - 1, j];
                        c.South = this[i + 1, j];
                        c.West = this[i, j - 1];
                        c.East = this[i, j + 1];
                    }
                }
            }
        }

        public override Cell RandomCell()
        {
            (int row, int col) = Mask.RandomLocation();
            return this[row, col];
        }

        public override int Size() => Mask.ActiveCellsCount;

    }
}