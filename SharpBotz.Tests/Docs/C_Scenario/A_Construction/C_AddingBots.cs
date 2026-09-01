using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Scenarios;

namespace SharpBotz.Tests.Docs.C_Scenario.A_Construction;

[DocFile]
public class C_AddingBots
{
    [Fact]
    [DocContent("This can be achieved in the following way:")]
    [DocExample(typeof(C_AddingBots), nameof(GetScenarioWithBots))]
    [DocContent("This creates:")]
    [DocExample(typeof(C_AddingBots), nameof(ExpectedGridWithBots), "text")]
    public void AddingBots()
    {
        var world = GetScenarioWithBots().Start();
        Assert.Equal(2, world.Bots.Count);
        Assert.Equal(new Position(1, 1), world.Bots[0].Position);
        Assert.Equal(Direction.Up, world.Bots[0].Facing);
    }

    [CodeSnippet]
    private static Scenario GetScenarioWithBots() =>
        Scenario.Named("Botz")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(3))
                .Build())
            .Spawn(() => new Bot(new DummyBrain(), ModuleRack.Create()))
                .At(1, 1)
                .Facing(Direction.Up)
            .Spawn(() => new Bot(new DummyBrain(), ModuleRack.Create()))
                .At(3, 1)
                .Facing(Direction.Up);

    [CodeSnippet]
    [ArenaGrid]
    private static ArenaTile[,] ExpectedGridWithBots() =>
        new ArenaTile[,]
        {
            { ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall },
            { ArenaTile.Wall, ArenaTile.BotDirectionUp, ArenaTile.Empty, ArenaTile.BotDirectionUp, ArenaTile.Wall },
            { ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall, ArenaTile.Wall }
        };

    // [Fact]
    // [DocContent("Placing a bot where a wall is already present:")]
    // [DocExample(typeof(C_AddingBots), nameof(GetArenaWithBotAlreadyExists))]
    // [DocContent("Throws a:")]
    // [DocExample(typeof(C_AddingBots), nameof(BotAlreadyExistsExceptionType))]
    // [DocContent("Containing the following message:")]
    // [DocExample(typeof(C_AddingBots), nameof(BotAlreadyExistsExceptionMessage), "text")]
    // public void AddingBotsThrowsIfOneExistsAlready()
    // {
    //     var ex = Assert.ThrowsAny<Exception>(() => GetArenaWithBotAlreadyExists());
    //     Assert.IsType(BotAlreadyExistsExceptionType(), ex);
    //     Assert.Equal(BotAlreadyExistsExceptionMessage(), ex.Message);
    // }

    // [CodeSnippet]
    // private static Arena GetArenaWithBotAlreadyExists() =>
    //     Arena
    //         .Create(
    //             ArenaWidth.Is(5),
    //             ArenaHeight.Is(3))
    //         .SpawnBotAt(new Bot(new DummyBrain(), ModuleRack.Create()), 1, 1)
    //         .SpawnBotAt(new Bot(new DummyBrain(), ModuleRack.Create()), 1, 1);

    // [CodeSnippet]
    // [CodeRemove("typeof(")]
    // [CodeRemove(");")]
    // private static Type BotAlreadyExistsExceptionType() =>
    //     typeof(ArenaConstructionException);

    // [CodeSnippet]
    // private static string BotAlreadyExistsExceptionMessage() =>
    //     "Tried adding a bot to a non empty tile at [1, 1].";
}

