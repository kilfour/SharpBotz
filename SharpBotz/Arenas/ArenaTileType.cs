namespace SharpBotz.Arenas;

public enum ArenaTileType
{
    Empty,
    Wall,
    OutOfBounds
}

public static class ArenaTileTypeExtensions
{
    public static ArenaTile ToArenaTile(this ArenaTileType arenaTileType) =>
        arenaTileType switch
        {
            ArenaTileType.Empty => ArenaTile.Empty,
            ArenaTileType.Wall => ArenaTile.Wall,
            ArenaTileType.OutOfBounds => ArenaTile.OutOfBounds,
            _ => throw new ArgumentOutOfRangeException(nameof(arenaTileType), arenaTileType, null),
        };
}