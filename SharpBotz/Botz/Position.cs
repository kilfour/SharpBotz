namespace SharpBotz.Botz;


public readonly struct Position(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public Position Move(Direction direction) => direction switch
    {
        Direction.Up => new Position(X, Y - 1),
        Direction.Down => new Position(X, Y + 1),
        Direction.Left => new Position(X - 1, Y),
        Direction.Right => new Position(X + 1, Y),
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
    };
}
