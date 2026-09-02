using QuickPulse.Explains;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.MeleeWeapons;

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
        new Melee(
            ModuleId.Is("melee"),
            damagePerPower: 10,
            maximumPower: 5);

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
        new(ModuleId.Is("melee"), damagePerPower, maximumPower);
}
