using SharpBotz.Botz;

namespace SharpBotz.Arenas;

public class Arena
{
    private List<BotPosition> botPositions = [];

    private readonly ArenaTileType[,] grid;

    private Arena(int width, int height)
    {
        grid = new ArenaTileType[width, height];
        InitializeGrid();
    }
    public static Arena Create(ArenaWidth width, ArenaHeight height)
        => new(width.Value, height.Value);

    private void InitializeGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
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

    public int Width => grid.GetLength(0);
    public int Height => grid.GetLength(1);

    public ArenaTileType this[int x, int y] => grid[x, y];

    public Arena AddWallAt(int x, int y)
    {
        if (grid[x, y] != ArenaTileType.Empty)
            throw new ArenaConstructionException($"Tried adding a wall to a non empty tile at [{x}, {y}].");
        grid[x, y] = ArenaTileType.Wall;
        return this;
    }

    public Arena SpawnBotAt(Bot bot, int x, int y)
    {
        if (grid[x, y] != ArenaTileType.Empty || botPositions.Any(a => a.X == x && a.Y == y))
            throw new ArenaConstructionException($"Tried adding a bot to a non empty tile at [{x}, {y}].");
        botPositions.Add(new(bot, x, y));
        return this;
    }

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
        foreach (var position in botPositions)
        {
            snapshot[position.X, position.Y] = position.Bot.Facing.ToBotDirectionTile();
        }
        return snapshot;
    }
}
