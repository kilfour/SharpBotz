using QuickPulse.Explains;
using SharpBotz.Arenas;

namespace SharpBotz.Tests.Docs.Arenas.A_Construction;

[DocFile]
public class B_AddingWalls
{
    [Fact]
    [DocContent("This can be achieved in the following way:")]
    [DocExample(typeof(B_AddingWalls), nameof(GetArenaWithWalls))]
    [DocContent("This creates:")]
    [DocExample(typeof(B_AddingWalls), nameof(ExpectedGridWithWalls), "text")]
    public void AddingWalls()
    {
        var arena = GetArenaWithWalls();
        var expectedGrid = ExpectedGridWithWalls();
        AssertGrid.Equal(expectedGrid, arena);
    }

    [CodeSnippet]
    private static Arena GetArenaWithWalls() =>
        Arena
            .Create(
                ArenaWidth.Is(5),
                ArenaHeight.Is(3))
            .AddWallAt(1, 1)
            .AddWallAt(3, 1);

    [CodeSnippet]
    [ArenaGrid]
    private static ArenaTileType[,] ExpectedGridWithWalls() =>
        new ArenaTileType[,]
        {
            { ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Wall },
            { ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Empty, ArenaTileType.Wall, ArenaTileType.Wall },
            { ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Wall }
        };

    [Fact]
    [DocContent("Placing a wall where one is already present:")]
    [DocExample(typeof(B_AddingWalls), nameof(GetArenaWitWallAlreadyExists))]
    [DocContent("Throws a:")]
    [DocExample(typeof(B_AddingWalls), nameof(WallAlreadyExistsExceptionType))]
    [DocContent("Containing the following message:")]
    [DocExample(typeof(B_AddingWalls), nameof(WallAlreadyExistsExceptionMessage), "text")]
    public void AddingWallsThrowsIfOneExistsAlready()
    {
        var ex = Assert.ThrowsAny<Exception>(() =>
             Arena
            .Create(
                ArenaWidth.Is(3),
                ArenaHeight.Is(3))
            .AddWallAt(1, 1)
            .AddWallAt(1, 1));
        Assert.IsType(WallAlreadyExistsExceptionType(), ex);
        Assert.Equal(WallAlreadyExistsExceptionMessage(), ex.Message);
    }

    [CodeSnippet]
    private static Arena GetArenaWitWallAlreadyExists() =>
        Arena
            .Create(
                ArenaWidth.Is(5),
                ArenaHeight.Is(3))
            .AddWallAt(1, 1)
            .AddWallAt(3, 1);

    [CodeSnippet]
    [CodeRemove("typeof(")]
    [CodeRemove(");")]
    private static Type WallAlreadyExistsExceptionType() =>
        typeof(ArenaConstructionException);

    [CodeSnippet]
    private static string WallAlreadyExistsExceptionMessage() =>
        "Tried adding a wall to non empty tile at [1, 1].";
}
