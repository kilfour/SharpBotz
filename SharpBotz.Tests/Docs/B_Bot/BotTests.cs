using QuickPulse.Explains;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Tests.Docs.B_Bot;

[DocFile]
[DocContent(
"""
A bot combines three things:

- a name that identifies it in the game world;
- a `BotBrain` that decides what it wants to do each turn; and
- a `ModuleRack` containing the hardware available to that brain.

Every bot starts with 100 hit points. It remains alive while it has at least one hit point.
""")]
public class BotTests
{
    [Fact]
    [DocHeader("Assembling A Bot")]
    [DocContent(
    """
    Use `Bot.Named`, `Brain`, and `Rack` to supply the three required parts in order.
    An empty module rack is valid and still contains the standard chassis, but a bot without modules cannot act:
    """)]
    [DocExample(typeof(BotTests), nameof(CreateBot))]
    [DocContent(
    """
    `IdleBrain` is the smallest valid brain. Returning an empty power plan makes the bot do nothing:
    """)]
    [DocExample(typeof(IdleBrain))]
    [DocContent(
    """
    The bot exposes its name, brain, and rack after construction.
    Module racks and bot brains are covered in detail in the following sections.
    """)]
    public void BotIsCreatedFromANameBrainAndRack()
    {
        var bot = CreateBot();

        Assert.Equal("starter", bot.Name);
        Assert.IsType<IdleBrain>(bot.Brain);
        Assert.Empty(bot.ModuleRack.GetModuleWeights());
        Assert.Equal(ModuleRack.ChassisWeight, bot.ModuleRack.TotalWeight);
        Assert.Equal(Bot.MaximumHitPoints, bot.HitPoints);
        Assert.True(bot.IsAlive);
    }

    [CodeSnippet]
    public static Bot CreateBot() =>
        Bot.Named("starter")
            .Brain(new IdleBrain())
            .Rack(ModuleRack.Create());

    [Fact]
    [DocHeader("Adding A Module")]
    [DocContent(
    """
    Install modules by passing them to `ModuleRack.Create`.
    Every module has a unique ID and adds its own weight to the standard chassis:
    """)]
    [DocExample(typeof(BotTests), nameof(CreatePoweredRack))]
    [DocContent(
    """
    Weight is an important design constraint. A scenario can reject a bot that exceeds its maximum weight, and a heavier bot requires more power from its thrusters to move.

    Call `ModuleRack.GetModuleWeights()` to inspect a rack's configuration. It returns a dictionary that maps each module ID to that module's weight:
    """)]
    [DocExample(typeof(BotTests), nameof(InspectConfiguration))]
    [DocContent(
    """
    In this configuration the reactor weighs 6 and the chassis weighs 10, giving the rack a total weight of 16.
    """)]
    public void InstalledModuleContributesToRackWeight()
    {
        var rack = CreatePoweredRack();
        var moduleWeights = InspectConfiguration(rack);

        Assert.Equal(6, moduleWeights["reactor"]);
        Assert.Equal(
            ModuleRack.ChassisWeight + moduleWeights.Values.Sum(),
            rack.TotalWeight);
        Assert.Equal(16, rack.TotalWeight);
    }

    [CodeSnippet]
    public static ModuleRack CreatePoweredRack() =>
        ModuleRack.Create(
            Reactor.Named("reactor")
                .MaximumOutput(3));

    [CodeSnippet]
    public static Dictionary<string, int> InspectConfiguration(ModuleRack rack) =>
        rack.GetModuleWeights();

    [Fact]
    [DocHeader("Defining A Reusable Bot Type")]
    [DocContent(
    """
    For a bot that will be created repeatedly, derive a class from `Bot` and pass the same three parts to its base constructor.
    `nameof` keeps the displayed bot name in sync with the class name:
    """)]
    [DocExample(typeof(StarterBot))]
    [DocContent(
    """
    Construct a fresh brain, rack, and set of modules for every bot instance.
    A module rack belongs to one bot and cannot be shared by several bots.
    """)]
    public void DerivedBotCreatesFreshPartsForEveryInstance()
    {
        var first = new StarterBot();
        var second = new StarterBot();

        Assert.Equal(nameof(StarterBot), first.Name);
        Assert.IsType<IdleBrain>(first.Brain);
        Assert.NotSame(first.Brain, second.Brain);
        Assert.NotSame(first.ModuleRack, second.ModuleRack);
    }

    [CodeExample]
    public class StarterBot() : Bot(
        nameof(StarterBot),
        new IdleBrain(),
        ModuleRack.Create());

    [CodeExample]
    public class IdleBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
