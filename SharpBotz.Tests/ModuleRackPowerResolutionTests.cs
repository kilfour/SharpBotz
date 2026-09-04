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

        var exception = Assert.Throws<ArgumentException>(() => PowerPlan.From(
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

        var exception = Assert.Throws<ArgumentException>(() => PowerPlan.From(
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
        var melee = Melee.Named("melee")
            .DamagePerPower(10)
            .MaximumPower(1);
        var rack = ModuleRack.Create(
            Reactor.Named("reactor").MaximumOutput(2),
            Battery.Named("battery").Capacity(10),
            melee);
        var modules = rack.GetModuleControl();
        var reactor = modules.RequireModule<ReactorInfo>();
        var meleeInfo = modules.RequireModule<MeleeInfo>();
        var plan = PowerPlan.From(
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

    [Fact]
    public void TwoRemainingPowerWithoutBatteriesCreatesAnEffectForTwoOfThreeReactors()
    {
        var (rack, reactorInfos, meleeInfo) = CreateRackWithoutBatteries();
        var plan = PowerPlan.From(
            reactorInfos[0].SetOutput(1),
            reactorInfos[1].SetOutput(1),
            reactorInfos[2].SetOutput(1),
            meleeInfo.Hit(1));

        var effects = rack.Resolve(plan).OfType<PowerCannotBeStoredEffect>();

        Assert.Equal(
            [
                new PowerCannotBeStoredEffect(reactorInfos[0].Id, ExcessPower: 1),
                new PowerCannotBeStoredEffect(reactorInfos[1].Id, ExcessPower: 1)
            ],
            effects);
    }

    [Fact]
    public void FourRemainingPowerWithoutBatteriesCreatesAnEffectForEveryReactor()
    {
        var (rack, reactorInfos, meleeInfo) = CreateRackWithoutBatteries();
        var plan = PowerPlan.From(
            reactorInfos[0].SetOutput(2),
            reactorInfos[1].SetOutput(2),
            reactorInfos[2].SetOutput(2),
            meleeInfo.Hit(2));

        var effects = rack.Resolve(plan).OfType<PowerCannotBeStoredEffect>();

        Assert.Equal(
            [
                new PowerCannotBeStoredEffect(reactorInfos[0].Id, ExcessPower: 2),
                new PowerCannotBeStoredEffect(reactorInfos[1].Id, ExcessPower: 1),
                new PowerCannotBeStoredEffect(reactorInfos[2].Id, ExcessPower: 1)
            ],
            effects);
    }

    [Fact]
    public void RemainingPowerIsDistributedEvenlyAcrossBatteries()
    {
        var rack = ModuleRack.Create(
            Reactor.Named("reactor").MaximumOutput(2),
            Battery.Named("battery-one").Capacity(10),
            Battery.Named("battery-two").Capacity(10),
            Battery.Named("battery-three").Capacity(10));
        var modules = rack.GetModuleControl();
        var reactor = modules.RequireModule<ReactorInfo>();

        rack.Resolve(PowerPlan.From(reactor.SetOutput(2)));

        Assert.Equal(
            [1, 1, 0],
            rack.GetModuleControl()
                .FindModules<BatteryInfo>()
                .Select(battery => battery.Charge));
    }

    private static (ModuleRack Rack, IReadOnlyList<ReactorInfo> Reactors, MeleeInfo Melee)
        CreateRackWithoutBatteries()
    {
        var rack = ModuleRack.Create(
            Reactor.Named("reactor-one").MaximumOutput(2),
            Reactor.Named("reactor-two").MaximumOutput(2),
            Reactor.Named("reactor-three").MaximumOutput(2),
            Melee.Named("melee")
                .DamagePerPower(1)
                .MaximumPower(2));
        var modules = rack.GetModuleControl();

        return (
            rack,
            modules.FindModules<ReactorInfo>(),
            modules.RequireModule<MeleeInfo>());
    }
}
