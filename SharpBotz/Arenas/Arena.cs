namespace SharpBotz.Arenas;

public class Arena
{
    // private static readonly ScanResult EmptyScanResult = new ScanResult.Empty();
    // private static readonly ScanResult WallScanResult = new ScanResult.Wall();
    // private static readonly ScanResult OutOfBoundsScanResult = new ScanResult.OutOfBounds();

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
            throw new ArenaConstructionException($"Tried adding a wall to non empty tile at [{x}, {y}].");
        grid[x, y] = ArenaTileType.Wall;
        return this;
    }

    // internal ArenaTileType[,] GetSlice(Position center, int size)
    // {
    //     if (size < 1)
    //     {
    //         throw new ArgumentOutOfRangeException(nameof(size), size, "Slice size must be positive.");
    //     }

    //     var slice = new ArenaTileType[size, size];
    //     var offset = size / 2;

    //     for (var sliceX = 0; sliceX < size; sliceX++)
    //     {
    //         for (var sliceY = 0; sliceY < size; sliceY++)
    //         {
    //             var arenaX = center.X - offset + sliceX;
    //             var arenaY = center.Y - offset + sliceY;
    //             slice[sliceX, sliceY] = IsInBounds(arenaX, arenaY)
    //                 ? grid[arenaX, arenaY]
    //                 : ArenaTileType.OutOfBounds;
    //         }
    //     }

    //     return slice;
    // }

    // internal ScanResult[,] GetScan(
    //     Bot observer,
    //     int size,
    //     ILookup<(int X, int Y), Bot> botsByPosition)
    // {
    //     ArgumentNullException.ThrowIfNull(observer);
    //     ArgumentNullException.ThrowIfNull(botsByPosition);

    //     var result = new ScanResult[size, size];
    //     var offset = size / 2;

    //     for (var scanX = 0; scanX < size; scanX++)
    //     {
    //         for (var scanY = 0; scanY < size; scanY++)
    //         {
    //             if (scanX == offset && scanY == offset)
    //             {
    //                 result[scanX, scanY] =
    //                     new ScanResult.OwnBot(observer.Facing, observer.HitPoints);
    //                 continue;
    //             }

    //             var arenaX = observer.Position.X - offset + scanX;
    //             var arenaY = observer.Position.Y - offset + scanY;
    //             var tile = IsInBounds(arenaX, arenaY)
    //                 ? grid[arenaX, arenaY]
    //                 : ArenaTileType.OutOfBounds;
    //             result[scanX, scanY] = CreateScanResult(
    //                 tile,
    //                 botsByPosition[(arenaX, arenaY)].LastOrDefault());
    //         }
    //     }

    //     return result;
    // }

    // private static ScanResult CreateScanResult(
    //     ArenaTileType tile,
    //     Bot? scannedBot) =>
    //     tile switch
    //     {
    //         ArenaTileType.Empty => EmptyScanResult,
    //         ArenaTileType.Wall => WallScanResult,
    //         ArenaTileType.OutOfBounds => OutOfBoundsScanResult,
    //         _ when scannedBot is not null =>
    //             new ScanResult.Bot(scannedBot.Name, scannedBot.Facing, scannedBot.HitPoints),
    //         _ => throw new InvalidOperationException(
    //             "The arena contains a bot tile without a corresponding bot."),
    //     };

    // internal IReadOnlyList<Position> GetAvailableSpawnPositions(IEnumerable<Bot> bots)
    // {
    //     ArgumentNullException.ThrowIfNull(bots);

    //     var occupiedPositions = bots
    //         .Where(bot => bot.HasSpawned)
    //         .Select(bot => bot.Position)
    //         .ToHashSet();

    //     return (
    //         from x in Enumerable.Range(1, Math.Max(0, Width - 2))
    //         from y in Enumerable.Range(1, Math.Max(0, Height - 2))
    //         let position = new Position(x, y)
    //         where grid[x, y] == ArenaTileType.Empty
    //         where !occupiedPositions.Contains(position)
    //         select position)
    //         .ToArray();
    // }

    // private void DrawBot(Bot bot)
    // {
    //     var position = bot.Position;
    //     grid[position.X, position.Y] = bot.Facing.ToBotDirectionTileType();
    // }

    // public void RedrawBots(IEnumerable<Bot> bots)
    // {
    //     ClearBotTiles();
    //     DrawBots(bots);
    // }

    // private void DrawBots(IEnumerable<Bot> bots)
    // {
    //     foreach (var bot in bots.Where(bot => bot.IsAlive))
    //     {
    //         DrawBot(bot);
    //     }
    // }

    // private void ClearBotTiles()
    // {
    //     for (var x = 0; x < Width; x++)
    //     {
    //         for (var y = 0; y < Height; y++)
    //         {
    //             if (grid[x, y].IsBotTile())
    //             {
    //                 grid[x, y] = ArenaTileType.Empty;
    //             }
    //         }
    //     }
    // }

    // public bool IsTyleType(Position position, ArenaTileType tileType) =>
    //     IsInBounds(position.X, position.Y) &&
    //     grid[position.X, position.Y] == tileType;

    // internal bool IsTraversable(Position position) =>
    //     IsInBounds(position.X, position.Y) &&
    //     grid[position.X, position.Y] != ArenaTileType.Wall;

    // private bool IsInBounds(int x, int y) =>
    //     x >= 0 &&
    //     x < Width &&
    //     y >= 0 &&
    //     y < Height;
}
