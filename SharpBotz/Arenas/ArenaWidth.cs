namespace SharpBotz.Arenas;

public readonly record struct ArenaWidth
{
    public int Value { get; }
    private ArenaWidth(int value) { Value = value; }
    public static ArenaWidth Is(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 3);
        return new(value);
    }
}
