using SharpBotz.Botz;

namespace SharpBotz.Arenas;


public class Arena
{
    private readonly ArenaTileType[,] grid;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public ArenaTileType this[int x, int y] => grid[x, y];

    public bool IsTraversable(Position position) =>
        position.X >= 0 &&
        position.X < Width &&
        position.Y >= 0 &&
        position.Y < Height &&
        grid[position.X, position.Y] == ArenaTileType.Empty;

    public ArenaTile[,] GetGrid()
    {
        var snapshot = new ArenaTile[Width, Height];
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                snapshot[x, y] = grid[x, y].ToArenaTile();
            }
        }
        return snapshot;
    }
    private Arena(ArenaTileType[,] grid)
    {
        this.grid = (ArenaTileType[,])grid.Clone();
        Width = grid.GetLength(0);
        Height = grid.GetLength(1);
    }

    public static ArenaBuilder Sized(ArenaWidth width, ArenaHeight height)
        => new(width.Value, height.Value);

    public class ArenaBuilder
    {
        public ArenaBuilder(int width, int height)
        {
            grid = new ArenaTileType[width, height];
            this.width = width;
            this.height = height;
            InitializeGrid();
        }

        private readonly ArenaTileType[,] grid;
        private readonly int width;
        private readonly int height;

        private void InitializeGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        grid[x, y] = ArenaTileType.Wall;
                    }
                    else
                    {
                        grid[x, y] = ArenaTileType.Empty;
                    }
                }
            }
        }

        public ArenaBuilder AddWallAt(int x, int y)
        {
            if (grid[x, y] != ArenaTileType.Empty)
                throw new ArenaConstructionException($"Tried adding a wall to a non empty tile at [{x}, {y}].");
            grid[x, y] = ArenaTileType.Wall;
            return this;
        }

        public Arena Build() => new(grid);
    }
}
