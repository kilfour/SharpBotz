using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Scenarios;

namespace SharpBotz.Tests;


public class Spike
{
    [Fact]
    public void BotGeneratePowerAndStore()
    {
        var rack =
            ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(10),
                Battery.Named("battery").Capacity(50)
            );
        var control = rack.GetModuleControl();
        var plan = new PowerPlan(control.RequireModule<ReactorInfo>().SetOutput(10));
        var effects = rack.Resolve(plan);
        Assert.Empty(effects);
        Assert.Equal(10, rack.BatteryLevel);
    }

    [Fact]
    public void BotOverload()
    {
        var rack =
            ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(10),
                Battery.Named("battery").Capacity(5)
            );
        var control = rack.GetModuleControl();
        var plan = new PowerPlan(control.RequireModule<ReactorInfo>().SetOutput(10));
        var effects = rack.Resolve(plan);
        var effect = Assert.Single(effects);
        Assert.IsType<BatteryOverChargedEffect>(effect);
        Assert.Equal(0, rack.BatteryLevel);
    }

    [Fact]
    public void BotOverloadScenario()
    {
        var rack =
            ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(10),
                Battery.Named("battery").Capacity(5)
            );
        var world = Scenario.Named("overload")
            .Arena(Arena.Sized(ArenaWidth.Is(3), ArenaHeight.Is(3)).Build())
            .MaximumTurns(1)
            .CompletesWhen(_ => false)
            .Spawn(() => new Bot(new OverLoadBrain(), rack)).At(1, 1).Facing(Direction.Up)
            .CreateWorld();
        world.Update();
        var bot = Assert.Single(world.Bots).Bot;
        Assert.Equal(90, bot.HitPoints);
        Assert.Equal(0, rack.BatteryLevel);
    }

    public class OverLoadBrain : BotBrain
    {
        protected override PowerPlan RoutePower(ModuleControl modules, BotObservation observation) =>
            new(modules.RequireModule<ReactorInfo>().SetOutput(15));
    }

    [Fact]
    public void BotMove()
    {
        var rack =
            ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(15),
                Battery.Named("battery").Capacity(10),
                Drive.Named("drive").ThrustPerPower(14).MaximumPower(4)
            );
        Assert.Equal(54, rack.TotalWeight);
        var control = rack.GetModuleControl();
        var plan = new PowerPlan(
            control.RequireModule<ReactorInfo>().SetOutput(4),
            control.RequireModule<DrivingInfo>().Move(1));
        var effects = rack.Resolve(plan);
        var effect = Assert.Single(effects);
        var thrustEffect = Assert.IsType<DriveEffect>(effect);
        Assert.Equal(1, thrustEffect.Speed);
        Assert.Equal(0, rack.BatteryLevel);
    }

    [Fact]
    public void BotDrained()
    {
        var rack =
            ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(10),
                Battery.Named("battery").Capacity(5),
                Drive.Named("drive").ThrustPerPower(5).MaximumPower(10)
            );
        var control = rack.GetModuleControl();
        var plan = new PowerPlan(
            control.RequireModule<ReactorInfo>().SetOutput(1),
            control.RequireModule<DrivingInfo>().Move(1));
        var effects = rack.Resolve(plan);
        var effect = Assert.Single(effects);
        Assert.IsType<BatteryDrainedEffect>(effect);
        Assert.Equal(0, rack.BatteryLevel);
    }
}
