namespace SharpBotz.Botz;

public sealed class BotScan
{
    private readonly ScanResult[,] results;

    public BotScan(ScanResult[,] results)
    {
        ArgumentNullException.ThrowIfNull(results);
        if (results.GetLength(0) != results.GetLength(1))
        {
            throw new ArgumentException(
                "A bot scan must be square.",
                nameof(results));
        }
        if (results.Length > 0 && results.GetLength(0) % 2 == 0)
        {
            throw new ArgumentException(
                "A bot scan must have an odd size.",
                nameof(results));
        }

        this.results = results;
        Size = results.GetLength(0);
        Range = Size / 2;
    }

    public int Range { get; }

    public int Size { get; }

    public ScanResult this[int x, int y]
    {
        get
        {
            if (Size == 0 || x < -Range || x > Range)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < -Range || y > Range)
                throw new ArgumentOutOfRangeException(nameof(y));

            return results[x + Range, y + Range];
        }
    }
}
