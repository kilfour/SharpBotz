using QuickPulse.Explains;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.RangedWeapons;

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
        new Ranged(
            ModuleId.Is("ranged"),
            range: 3,
            damagePerPower: 10,
            maximumPower: 5);

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
    A ranged weapon's base weight is 3.
    Increasing range adds weight following the triangular number curve.
    """)]
    [DocBarChart(
        typeof(E_RangedTests),
        nameof(RangeWeightCurve),
        "Weight by range",
        "Range",
        "Weight",
        0, 19)]
    [DocContent(
    """
    Higher damage per power adds weight following a squared curve.
    This example uses a range of 1 and maximum power of 10.
    """)]
    [DocBarChart(
        typeof(E_RangedTests),
        nameof(DamagePerPowerWeightCurve),
        "Weight by damage per power",
        "Damage Per Power",
        "Weight",
        0, 8)]
    [DocContent(
    """
    Maximum power also affects efficiency. This example uses a range of 3 and damage per power of 20.
    """)]
    [DocBarChart(
        typeof(E_RangedTests),
        nameof(MaximumPowerWeightCurve),
        "Weight by maximum power",
        "Maximum Power",
        "Weight",
        0, 22)]
    public void WeightCurveTest()
    {
        foreach (var (range, weight) in RangeWeightCurve)
        {
            Assert.Equal(weight, CreateRanged(range, damagePerPower: 10, maximumPower: 5).Weight);
        }

        foreach (var (damagePerPower, weight) in DamagePerPowerWeightCurve)
        {
            Assert.Equal(weight, CreateRanged(range: 1, damagePerPower, maximumPower: 10).Weight);
        }

        foreach (var (maximumPower, weight) in MaximumPowerWeightCurve)
        {
            Assert.Equal(weight, CreateRanged(range: 3, damagePerPower: 20, maximumPower).Weight);
        }
    }

    private static readonly (int Range, int Weight)[] RangeWeightCurve =
        [
            (1, 5),
            (2, 7),
            (3, 10),
            (4, 14),
            (5, 19)
        ];

    private static readonly (int DamagePerPower, int Weight)[] DamagePerPowerWeightCurve =
        [
            (1, 5),
            (5, 5),
            (10, 5),
            (15, 7),
            (20, 8)
        ];

    private static readonly (int MaximumPower, int Weight)[] MaximumPowerWeightCurve =
        [
            (1, 22),
            (2, 17),
            (3, 16),
            (4, 15),
            (5, 14),
            (6, 14),
            (10, 13)
        ];

    private static Ranged CreateRanged(int range, int damagePerPower, int maximumPower) =>
        new(ModuleId.Is("ranged"), range, damagePerPower, maximumPower);
}
