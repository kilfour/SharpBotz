using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Tests;

// Steps
//   1. Generate Power
//   2. Scan
//   2. Rotate
//   3. Move
//   4. Weapons
//   5. Store Power
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
        var effects = rack.Translate(plan);
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
        var effects = rack.Translate(plan);
        var effect = Assert.Single(effects);
        Assert.IsType<BatteryOverChargedEffect>(effect);
        Assert.Equal(0, rack.BatteryLevel);
    }
}