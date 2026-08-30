namespace SharpBotz.Arenas;

public readonly record struct ArenaHeight
{
    public int Value { get; }
    private ArenaHeight(int value) { Value = value; }
    public static ArenaHeight Is(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 3);
        return new(value);
    }
}
