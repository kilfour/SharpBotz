using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Scenarios;

namespace SharpBotz.Tests.Docs.C_Scenario.A_Construction;

[DocFile]
public class D_MaximumBotWeight
{
    [Fact]
    [DocContent("A Scenario can define the maximum bot weight allowed.")]
    [DocExample(typeof(D_MaximumBotWeight), nameof(GetScenario))]
    [DocContent("Using the following bot in that scenario:")]
    [DocExample(typeof(D_MaximumBotWeight), nameof(HeavyBot))]
    [DocContent("Causes `CreateWorld` to throw:")]
    [DocExample(typeof(D_MaximumBotWeight), nameof(BotTooHeavyException))]
    [DocExample(typeof(D_MaximumBotWeight), nameof(BotTooHeavyExceptionMessage))]
    public void Construction()
    {
        var arena =
            Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build();
        var scenario =
            GetScenario(arena)
                .Spawn(HeavyBot)
                .At(1, 1)
                .Facing(Direction.Up);
        var ex = Assert.ThrowsAny<Exception>(() => scenario.CreateWorld());
        Assert.IsType(BotTooHeavyException(), ex);
        Assert.Equal(BotTooHeavyExceptionMessage(), ex.Message);
    }

    [CodeSnippet]
    private static Scenario GetScenario(Arena arena) =>
        Scenario.Named("My Scenario")
            .Arena(arena)
            .MaximumTurns(20)
            .CompletesWhen(_ => false)
            .MaximumBotWeight(1);


    [CodeSnippet]
    private static Bot HeavyBot() =>
        Bot.Named("Heavy")
            .Brain(new DummyBrain())
            .Rack(ModuleRack.Create(
                Drive.Named("drive")
                    .ThrustPerPower(10)
                    .MaximumPower(5)));

    [CodeSnippet]
    [CodeRemove("typeof(")]
    [CodeRemove(");")]
    private static Type BotTooHeavyException() =>
        typeof(ArgumentException);

    [CodeSnippet]
    private static string BotTooHeavyExceptionMessage() =>
         $"A bot cannot weigh more than 1. Heavy's module rack weighs 28. (Parameter 'placement')";
}
