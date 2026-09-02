using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Rotators;
using SharpBotz.Worlds;

namespace SharpBotz.Tests;

public class GameWorldRotatorEffectsTests
{
    [Fact]
    public void OverchargedRotatorStillTurnsAndDamagesItsBot()
    {
        var world = new GameWorld(
            Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build(),
            [
                new BotState(
                    new Bot(
                        new TurningBrain(),
                        ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(2),
                            Battery.Named("battery").Capacity(10),
                            Rotator.Named("rotator")
                                .TorquePerPower(20)
                                .MaximumPower(1)
                                .Right())),
                    new Position(1, 1),
                    Direction.Up)
            ]);

        world.Update();

        Assert.Equal(Direction.Right, world.Bots[0].Facing);
        Assert.Equal(97, world.Bots[0].Bot.HitPoints);
    }

    private class TurningBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var reactor = modules.RequireModule<ReactorInfo>();
            var rotator = modules.RequireModule<RightRotatorInfo>();

            return new(
                reactor.SetOutput(2),
                new PowerAllocation(rotator.Id, 2));
        }
    }
}
