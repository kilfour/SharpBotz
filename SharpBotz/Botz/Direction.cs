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

    public static ArenaTileType ToBotDirectionTileType(this Direction direction) => direction switch
    {
        Direction.Up => ArenaTileType.BotDirectionUp,
        Direction.Down => ArenaTileType.BotDirectionDown,
        Direction.Left => ArenaTileType.BotDirectionLeft,
        Direction.Right => ArenaTileType.BotDirectionRight,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
}
