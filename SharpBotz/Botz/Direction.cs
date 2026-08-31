using SharpBotz.Arenas;

namespace SharpBotz.Botz;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static Direction RotateLeft(this Direction direction) => direction switch
    {
        Direction.Up => Direction.Left,
        Direction.Left => Direction.Down,
        Direction.Down => Direction.Right,
        Direction.Right => Direction.Up,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
    };

    public static Direction RotateRight(this Direction direction) => direction switch
    {
        Direction.Up => Direction.Right,
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
    };

    public static ArenaTile ToBotDirectionTile(this Direction direction) => direction switch
    {
        Direction.Up => ArenaTile.BotDirectionUp,
        Direction.Down => ArenaTile.BotDirectionDown,
        Direction.Left => ArenaTile.BotDirectionLeft,
        Direction.Right => ArenaTile.BotDirectionRight,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
}
