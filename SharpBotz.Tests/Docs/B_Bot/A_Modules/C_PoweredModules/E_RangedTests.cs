using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.RangedWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Scenarios;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.C_PoweredModules;

[DocFile]
public class E_RangedTests
{
    [Fact]
    [DocContent(
    """
    A ranged weapon fires in the direction your bot is facing.
    The first bot in its path is hit, provided it is within range and no wall blocks the shot.


    It is created with its range, damage per power and maximum power, along with a ModuleId.
    """)]
    [DocExample(typeof(E_RangedTests), nameof(ConstructionExample))]
    public void Construction()
    {
        var ranged = ConstructionExample();
        var info = (RangedInfo)ranged.GetInfo(totalWeight: 0);

        Assert.Equal("ranged", ranged.Id.ToString());
        Assert.Equal(3, info.Range);
        Assert.Equal(10, info.DamagePerPower);
        Assert.Equal(5, info.MaximumPower);
    }

    [CodeSnippet]
    private static Ranged ConstructionExample() =>
        Ranged.Named("ranged")
            .Range(3)
            .DamagePerPower(10)
            .MaximumPower(5);

    [Fact]
    [DocContent(
    """
    Call `Fire` on the module info from your BotBrain to request a shot.
    The requested damage is rounded up to the next whole unit of power.
    """)]
    public void PowerConsumption()
    {
        var info = (RangedInfo)ConstructionExample().GetInfo(totalWeight: 0);

        Assert.Equal(1, info.Fire(10).Power);
        Assert.Equal(2, info.Fire(11).Power);
        Assert.Equal(3, info.Fire(25).Power);
    }

    [Fact]
    [DocContent(
    """
    Supplying more than the ranged weapon's maximum power overcharges it.
    The shot still lands, but every excess unit of power deals 3 damage to the attacking bot.
    """)]
    public void Overcharge()
    {
        var world = CreateOverchargedWorld();

        world.Update();

        Assert.Equal(97, world.Bots[0].Bot.HitPoints);
        Assert.Equal(80, world.Bots[1].Bot.HitPoints);
    }

    [CodeSnippet]
    private static GameWorld CreateOverchargedWorld() =>
        Scenario.Named("Overcharged ranged weapon")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(1)
            .CompletesWhen(_ => false)
            .Spawn(() => new Bot(
                        new OverchargedRangedBrain(),
                        ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(2),
                            Battery.Named("battery").Capacity(10),
                            Ranged.Named("ranged")
                                .Range(3)
                                .DamagePerPower(10)
                                .MaximumPower(1))))
                .At(1, 1)
                .Facing(Direction.Right)
            .Spawn(() => new Bot(
                        new IdleBrain(),
                        ModuleRack.Create(
                            Battery.Named("battery").Capacity(10))))
                .At(3, 1)
                .Facing(Direction.Left)
            .CreateWorld();

    [Fact]
    [DocContent(
    """
    A ranged weapon's base weight is 3.
    Increasing range adds weight following the triangular number curve.
    """)]
    [DocBarChart(
        typeof(E_RangedTests),
        nameof(RangeWeightCurve),
        "Weight by range",
        "Range",
        "Weight",
        0, 20)]
    [DocContent(
    """
    Higher damage per power adds weight following a squared curve.
    This example uses a range of 1 and maximum power of 1.
    """)]
    [DocBarChart(
        typeof(E_RangedTests),
        nameof(DamagePerPowerWeightCurve),
        "Weight by damage per power",
        "Damage Per Power",
        "Weight",
        0, 9)]
    [DocContent(
    """
    Supporting more power adds weight following the triangular number curve.
    This example uses a range of 3 and damage per power of 20.
    """)]
    [DocBarChart(
        typeof(E_RangedTests),
        nameof(MaximumPowerWeightCurve),
        "Weight by maximum power",
        "Maximum Power",
        "Weight",
        0, 28)]
    public void WeightCurveTest()
    {
        foreach (var (range, weight) in RangeWeightCurve)
        {
            Assert.Equal(weight, CreateRanged(range, damagePerPower: 10, maximumPower: 1).Weight);
        }

        foreach (var (damagePerPower, weight) in DamagePerPowerWeightCurve)
        {
            Assert.Equal(weight, CreateRanged(range: 1, damagePerPower, maximumPower: 1).Weight);
        }

        foreach (var (maximumPower, weight) in MaximumPowerWeightCurve)
        {
            Assert.Equal(weight, CreateRanged(range: 3, damagePerPower: 20, maximumPower).Weight);
        }
    }

    private static readonly (int Range, int Weight)[] RangeWeightCurve =
        [
            (1, 6),
            (2, 8),
            (3, 11),
            (4, 15),
            (5, 20)
        ];

    private static readonly (int DamagePerPower, int Weight)[] DamagePerPowerWeightCurve =
        [
            (1, 6),
            (5, 6),
            (10, 6),
            (15, 8),
            (20, 9)
        ];

    private static readonly (int MaximumPower, int Weight)[] MaximumPowerWeightCurve =
        [
            (1, 14),
            (2, 16),
            (3, 19),
            (4, 23),
            (5, 28)
        ];

    private static Ranged CreateRanged(int range, int damagePerPower, int maximumPower) =>
        Ranged.Named("ranged")
            .Range(range)
            .DamagePerPower(damagePerPower)
            .MaximumPower(maximumPower);

    private class OverchargedRangedBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var reactor = modules.RequireModule<ReactorInfo>();
            var shot = modules.RequireModule<RangedInfo>().Fire(damage: 20);

            return new(
                reactor.SetOutput(shot.Power),
                shot);
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
