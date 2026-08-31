using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Tests.Docs.A_Arena.A_Construction;

[DocFile]
public class C_AddingBots
{
    [Fact]
    [DocContent("This can be achieved in the following way:")]
    [DocExample(typeof(C_AddingBots), nameof(GetArenaWithBots))]
    [DocContent("This creates:")]
    [DocExample(typeof(C_AddingBots), nameof(ExpectedGridWithBots), "text")]
    public void AddingBots()
    {
        var arena = GetArenaWithBots();
        var expectedGrid = ExpectedGridWithBots();
        AssertGrid.Equal(expectedGrid, arena);
    }

    [CodeSnippet]
    private static Arena GetArenaWithBots() =>
        Arena
            .Create(
                ArenaWidth.Is(5),
                ArenaHeight.Is(3))
            .SpawnBotAt(new Bot(new DummyBrain(), ModuleRack.Create()), 1, 1)
            .SpawnBotAt(new Bot(new DummyBrain(), ModuleRack.Create()), 3, 1);

    [CodeSnippet]
    [ArenaGrid]
    private static ArenaTile[,] ExpectedGridWithBots() =>
        new ArenaTile[,]
        {
            { ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall },
            { ArenaTile.Wall, ArenaTile.BotDirectionUp, ArenaTile.Empty, ArenaTile.BotDirectionUp, ArenaTile.Wall },
            { ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall }
        };

    [Fact]
    [DocContent("Placing a bot where a wall is already present:")]
    [DocExample(typeof(C_AddingBots), nameof(GetArenaWithBotAlreadyExists))]
    [DocContent("Throws a:")]
    [DocExample(typeof(C_AddingBots), nameof(BotAlreadyExistsExceptionType))]
    [DocContent("Containing the following message:")]
    [DocExample(typeof(C_AddingBots), nameof(BotAlreadyExistsExceptionMessage), "text")]
    public void AddingBotsThrowsIfOneExistsAlready()
    {
        var ex = Assert.ThrowsAny<Exception>(() => GetArenaWithBotAlreadyExists());
        Assert.IsType(BotAlreadyExistsExceptionType(), ex);
        Assert.Equal(BotAlreadyExistsExceptionMessage(), ex.Message);
    }

    [CodeSnippet]
    private static Arena GetArenaWithBotAlreadyExists() =>
        Arena
            .Create(
                ArenaWidth.Is(5),
                ArenaHeight.Is(3))
            .SpawnBotAt(new Bot(new DummyBrain(), ModuleRack.Create()), 1, 1)
            .SpawnBotAt(new Bot(new DummyBrain(), ModuleRack.Create()), 1, 1);

    [CodeSnippet]
    [CodeRemove("typeof(")]
    [CodeRemove(");")]
    private static Type BotAlreadyExistsExceptionType() =>
        typeof(ArenaConstructionException);

    [CodeSnippet]
    private static string BotAlreadyExistsExceptionMessage() =>
        "Tried adding a bot to a non empty tile at [1, 1].";
}

public class DummyBrain : BotBrain
{
    protected override PowerPlan RoutePower(BotObservation observation) =>
        PowerPlan.Empty;
}