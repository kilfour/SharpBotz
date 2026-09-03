using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Scenarios;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.B_Bot.B_BuildingABotBrain;

[DocFile]
[DocFileHeader("Building A Bot Brain")]
public class BotBrainTests
{
    [Fact]
    [DocContent(
    """
    A bot brain decides what its bot will do each turn.
    Create one by inheriting from `BotBrain` and implementing `RoutePower`.

    `ModuleControl` provides information about the bot's installed modules.
    Calling a module action creates a power intention; it does not immediately perform that action.
    Return those intentions together in a `PowerPlan`.

    This brain asks its drive to move one tile, then asks its reactor to generate exactly the power that movement requires:
    """)]
    [DocExample(typeof(MoveForwardBrain))]
    [DocContent(
    """
    The game world calls the brain once per turn and resolves the returned plan.
    `BotObservation` contains what the bot observed on the previous turn and can be used to make later brains react to their surroundings.
    """)]
    public void RoutesPowerToMoveForward()
    {
        var world = CreateWorld();

        world.Update();

        Assert.Equal(new Position(2, 1), world.Bots[0].Position);
    }

    [Fact]
    [DocContent(
    """
    A brain is installed in a bot together with the module rack it controls:
    """)]
    [DocExample(typeof(BotBrainTests), nameof(CreateBot))]
    public void BrainIsInstalledInABot()
    {
        var bot = CreateBot();

        Assert.IsType<MoveForwardBrain>(bot.Brain);
        Assert.Equal(1, bot.ModuleRack.MaximumReactorOutput);
    }

    [CodeExample]
    public static Bot CreateBot() =>
        new(
            new MoveForwardBrain(),
            ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(1),
                Drive.Named("drive")
                    .ThrustPerPower(100)
                    .MaximumPower(1)));

    private static GameWorld CreateWorld() =>
        Scenario.Named("Move forward")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(1)
            .CompletesWhen(_ => false)
            .Spawn(CreateBot)
                .At(1, 1)
                .Facing(Direction.Right)
            .CreateWorld(seed: 1234);

    [CodeExample]
    public class MoveForwardBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var reactor = modules.RequireModule<ReactorInfo>();
            var drive = modules.RequireModule<DrivingInfo>();
            var movement = drive.Move(speed: 1);

            return new PowerPlan(
                reactor.SetOutput(movement.Power),
                movement);
        }
    }
}
