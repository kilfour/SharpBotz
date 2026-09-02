using QuickPulse.Explains;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Scanners;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.C_PoweredModules;

[DocFile]
public class F_ScannerTests
{
    [Fact]
    [DocContent(
    """
    A scanner lets your bot observe a square area around itself on the following turn.


    It is created with the power required per unit of range and its maximum power, along with a ModuleId.
    """)]
    [DocExample(typeof(F_ScannerTests), nameof(ConstructionExample))]
    public void Construction()
    {
        var scanner = ConstructionExample();
        var info = (ScannerInfo)scanner.GetInfo(totalWeight: 0);

        Assert.Equal("scanner", scanner.Id.ToString());
        Assert.Equal(2, info.PowerPerRange);
        Assert.Equal(10, info.MaximumPower);
        Assert.Equal(5, info.MaximumRange);
    }

    [CodeSnippet]
    private static Scanner ConstructionExample() =>
        new Scanner(
            ModuleId.Is("scanner"),
            powerPerRange: 2,
            maximumPower: 10);

    [Fact]
    [DocContent(
    """
    Call `Scan` on the module info from your BotBrain to request a scan.
    Its power consumption is the requested range multiplied by power per range.
    """)]
    public void PowerConsumption()
    {
        var info = (ScannerInfo)ConstructionExample().GetInfo(totalWeight: 0);

        Assert.Equal(2, info.Scan(1).Power);
        Assert.Equal(6, info.Scan(3).Power);
        Assert.Equal(10, info.Scan(5).Power);
    }

    [Fact]
    [DocContent(
    """
    A scanner's base weight is 2 at the standard efficiency of 3 power per range.
    Reducing the power required per range adds weight.
    """)]
    [DocBarChart(
        typeof(F_ScannerTests),
        nameof(WeightCurve),
        "Weight by power per range",
        "Power Per Range",
        "Weight",
        0, 4)]
    public void WeightCurveTest()
    {
        foreach (var (powerPerRange, weight) in WeightCurve)
        {
            Assert.Equal(weight, CreateScanner(powerPerRange).Weight);
        }
    }

    private static readonly (int PowerPerRange, int Weight)[] WeightCurve =
        [
            (1, 4),
            (2, 3),
            (3, 2),
            (4, 2),
            (5, 2)
        ];

    private static Scanner CreateScanner(int powerPerRange) =>
        new(ModuleId.Is("scanner"), powerPerRange, maximumPower: 10);
}
