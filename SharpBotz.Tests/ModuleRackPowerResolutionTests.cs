using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Tests;

public class ModuleRackPowerResolutionTests
{
    [Fact]
    public void RepeatedGenerationsAreRejected()
    {
        var reactor = new ReactorInfo(ModuleId.Is("reactor"), MaximumOutput: 1);

        var exception = Assert.Throws<ArgumentException>(() => new PowerPlan(
            reactor.SetOutput(1),
            reactor.SetOutput(1)));

        Assert.Equal("intentions", exception.ParamName);
        Assert.StartsWith(
            "A module can only be activated once per power plan.",
            exception.Message);
    }

    [Fact]
    public void RepeatedAllocationsAreRejected()
    {
        var melee = new MeleeInfo(
            ModuleId.Is("melee"),
            DamagePerPower: 10,
            MaximumPower: 1);

        var exception = Assert.Throws<ArgumentException>(() => new PowerPlan(
            melee.Hit(10),
            melee.Hit(10)));

        Assert.Equal("intentions", exception.ParamName);
        Assert.StartsWith(
            "A module can only be activated once per power plan.",
            exception.Message);
    }

    [Fact]
    public void AnOverchargedAllocationStillCreatesTheModuleEffect()
    {
        var melee = new Melee(
            ModuleId.Is("melee"),
            damagePerPower: 10,
            maximumPower: 1);
        var rack = ModuleRack.Create(
            Reactor.Named("reactor").MaximumOutput(2),
            Battery.Named("battery").Capacity(10),
            melee);
        var modules = rack.GetModuleControl();
        var reactor = modules.RequireModule<ReactorInfo>();
        var meleeInfo = modules.RequireModule<MeleeInfo>();
        var plan = new PowerPlan(
            reactor.SetOutput(2),
            meleeInfo.Hit(20));

        var effects = rack.Resolve(plan);

        Assert.Collection(
            effects,
            effect => Assert.IsType<MeleeOverChargedEffect>(effect),
            effect => Assert.Equal(
                new MeleeEffect(melee.Id, Damage: 20),
                Assert.IsType<MeleeEffect>(effect)));
    }
}
