using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.C_PoweredModules;

[DocFile]
public class D_MeleeTests
{
    [Fact]
    [DocContent(
    """
    A melee weapon damages the bot directly in front of your bot.


    It is created with its damage per power and maximum power, along with a ModuleId.
    """)]
    [DocExample(typeof(D_MeleeTests), nameof(ConstructionExample))]
    public void Construction()
    {
        var melee = ConstructionExample();
        var info = (MeleeInfo)melee.GetInfo(totalWeight: 0);

        Assert.Equal("melee", melee.Id.ToString());
        Assert.Equal(10, info.DamagePerPower);
        Assert.Equal(5, info.MaximumPower);
    }

    [CodeSnippet]
    private static Melee ConstructionExample() =>
        Melee.Named("melee")
            .DamagePerPower(10)
            .MaximumPower(5);

    [Fact]
    [DocContent(
    """
    Call `Hit` on the module info from your BotBrain to request an attack.
    The requested damage is rounded up to the next whole unit of power.
    """)]
    public void PowerConsumption()
    {
        var info = (MeleeInfo)ConstructionExample().GetInfo(totalWeight: 0);

        Assert.Equal(1, info.Hit(10).Power);
        Assert.Equal(2, info.Hit(11).Power);
        Assert.Equal(3, info.Hit(25).Power);
    }

    [Fact]
    [DocContent(
    """
    Supplying more than the melee weapon's maximum power overcharges it.
    The attack still lands, but every excess unit of power deals 3 damage to the attacking bot.

    Here a weapon with maximum power 1 receives 2 power. It deals 20 damage and its bot takes 3 damage.
    """)]
    [DocExample(typeof(D_MeleeTests), nameof(CreateOverchargedWorld))]
    public void Overcharge()
    {
        var world = CreateOverchargedWorld();

        world.Update();

        Assert.Equal(97, world.Bots[0].Bot.HitPoints);
        Assert.Equal(80, world.Bots[1].Bot.HitPoints);
    }

    [CodeSnippet]
    private static GameWorld CreateOverchargedWorld() =>
        new GameWorld(
            Arena.Sized(
                    ArenaWidth.Is(4),
                    ArenaHeight.Is(3))
                .Build(),
            [
                new BotState(
                    new Bot(
                        new OverchargedMeleeBrain(),
                        ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(2),
                            Battery.Named("battery").Capacity(10),
                            Melee.Named("melee")
                                .DamagePerPower(10)
                                .MaximumPower(1))),
                    new Position(1, 1),
                    Direction.Right),
                new BotState(
                    new Bot(
                        new IdleBrain(),
                        ModuleRack.Create(
                            Battery.Named("battery").Capacity(10))),
                    new Position(2, 1),
                    Direction.Left)
            ]);

    [Fact]
    [DocContent(
    """
    A melee weapon's base weight is 2.
    Higher damage per power adds weight following a squared curve.
    """)]
    [DocBarChart(
        typeof(D_MeleeTests),
        nameof(DamagePerPowerWeightCurve),
        "Weight by damage per power",
        "Damage Per Power",
        "Weight",
        0, 10)]
    [DocContent(
    """
    Supporting more power adds weight following the triangular number curve.
    This example keeps damage per power at 20.
    """)]
    [DocBarChart(
        typeof(D_MeleeTests),
        nameof(MaximumPowerWeightCurve),
        "Weight by maximum power",
        "Maximum Power",
        "Weight",
        0, 21)]
    public void WeightCurveTest()
    {
        foreach (var (damagePerPower, weight) in DamagePerPowerWeightCurve)
        {
            Assert.Equal(weight, CreateMelee(damagePerPower, maximumPower: 1).Weight);
        }

        foreach (var (maximumPower, weight) in MaximumPowerWeightCurve)
        {
            Assert.Equal(weight, CreateMelee(damagePerPower: 20, maximumPower).Weight);
        }
    }

    private static readonly (int DamagePerPower, int Weight)[] DamagePerPowerWeightCurve =
        [
            (1, 4),
            (5, 4),
            (10, 4),
            (15, 6),
            (20, 7),
            (25, 10)
        ];

    private static readonly (int MaximumPower, int Weight)[] MaximumPowerWeightCurve =
        [
            (1, 7),
            (2, 9),
            (3, 12),
            (4, 16),
            (5, 21)
        ];

    private static Melee CreateMelee(int damagePerPower, int maximumPower) =>
        Melee.Named("melee")
            .DamagePerPower(damagePerPower)
            .MaximumPower(maximumPower);

    private sealed class OverchargedMeleeBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var reactor = modules.RequireModule<ReactorInfo>();
            var attack = modules.RequireModule<MeleeInfo>().Hit(damage: 20);

            return new(
                reactor.SetOutput(attack.Power),
                attack);
        }
    }

    private sealed class IdleBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
