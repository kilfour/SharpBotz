using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Worlds;

namespace SharpBotz.Tests;

public class GameWorldDriveEffectsTests
{
    [Fact]
    public void MovesIndependentBotsInTheSameTurn()
    {
        var world = CreateWorld(
            CreateState(1, 1, Direction.Right),
            CreateState(3, 3, Direction.Left));

        world.Update();

        Assert.Equal(new Position(2, 1), world.Bots[0].Position);
        Assert.Equal(new Position(2, 3), world.Bots[1].Position);
    }

    [Fact]
    public void DamagesAndStopsBotsThatTargetTheSamePosition()
    {
        var world = CreateWorld(
            CreateState(1, 2, Direction.Right),
            CreateState(3, 2, Direction.Left));

        world.Update();

        Assert.Equal(new Position(1, 2), world.Bots[0].Position);
        Assert.Equal(new Position(3, 2), world.Bots[1].Position);
        Assert.Equal(90, world.Bots[0].Bot.HitPoints);
        Assert.Equal(90, world.Bots[1].Bot.HitPoints);
    }

    [Fact]
    public void DamagesEachBotOnceDuringAHeadOnSwap()
    {
        var world = CreateWorld(
            CreateState(1, 2, Direction.Right),
            CreateState(2, 2, Direction.Left));

        world.Update();

        Assert.Equal(new Position(1, 2), world.Bots[0].Position);
        Assert.Equal(new Position(2, 2), world.Bots[1].Position);
        Assert.Equal(90, world.Bots[0].Bot.HitPoints);
        Assert.Equal(90, world.Bots[1].Bot.HitPoints);
    }

    [Fact]
    public void DamagesAndStopsABotThatTargetsAWall()
    {
        var world = CreateWorld(CreateState(1, 1, Direction.Up));

        world.Update();

        Assert.Equal(new Position(1, 1), world.Bots[0].Position);
        Assert.Equal(90, world.Bots[0].Bot.HitPoints);
    }

    [Fact]
    public void ResolvesMovementOneStepAtATime()
    {
        var world = CreateWorld(
            CreateState(1, 2, Direction.Right, speed: 2),
            CreateIdleState(3, 2, Direction.Right));

        world.Update();

        Assert.Equal(new Position(2, 2), world.Bots[0].Position);
        Assert.Equal(new Position(3, 2), world.Bots[1].Position);
        Assert.Equal(90, world.Bots[0].Bot.HitPoints);
        Assert.Equal(90, world.Bots[1].Bot.HitPoints);
    }

    [Fact]
    public void OverchargedDriveStillMovesAndDamagesItsBot()
    {
        var world = CreateWorld(
            new BotState(
                new Bot(
                    new MovingBrain(speed: 1),
                    ModuleRack.Create(
                        Reactor.Named("reactor").MaximumOutput(2),
                        Battery.Named("battery").Capacity(10),
                        Drive.Named("drive")
                            .ThrustPerPower(20)
                            .MaximumPower(1))),
                new Position(1, 1),
                Direction.Right));

        world.Update();

        Assert.Equal(new Position(2, 1), world.Bots[0].Position);
        Assert.Equal(97, world.Bots[0].Bot.HitPoints);
    }

    private static GameWorld CreateWorld(params BotState[] botStates) =>
        new(
            Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(5))
                .Build(),
            botStates,
            maximumTurns: 10,
            complete: _ => false,
            seed: 1234);

    private static BotState CreateState(
        int x,
        int y,
        Direction facing,
        int speed = 1) =>
        new(
            new Bot(
                new MovingBrain(speed),
                ModuleRack.Create(
                    Reactor.Named("reactor").MaximumOutput(2),
                    Battery.Named("battery").Capacity(10),
                    Drive.Named("drive").ThrustPerPower(100).MaximumPower(2))),
            new Position(x, y),
            facing);

    private static BotState CreateIdleState(int x, int y, Direction facing) =>
        new(
            new Bot(
                new IdleBrain(),
                ModuleRack.Create(
                    Battery.Named("battery").Capacity(10))),
            new Position(x, y),
            facing);

    private class MovingBrain(int speed) : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var movement = modules.RequireModule<DrivingInfo>().Move(speed);
            return new(
                modules.RequireModule<ReactorInfo>().SetOutput(movement.Power),
                movement);
        }
    }

    private class IdleBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
