using QuickPulse.Explains;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.A_Battery;


[DocFile]
public class A_BatteryTests
{

    [Fact]
    [DocContent("A Battery is created by passing in it's capacity along with a ModuleId.")]
    [DocExample(typeof(A_BatteryTests), nameof(BatteryConstructionExample))]
    [DocContent("It's initial Charge is set to zero.")]
    public void BatteryConstruction()
    {
        var battery = BatteryConstructionExample();
        Assert.Equal(100, battery.Capacity);
        Assert.Equal(100, battery.AvailableCapacity);
        Assert.Equal(0, battery.Charge);
    }

    [CodeSnippet]
    private static Battery BatteryConstructionExample() =>
         Battery.Create(ModuleId.Is("battery"), 100);

    [Fact]
    [DocContent("A Battery with a capacity of zero throws upon construction.")]
    public void Invalid()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Battery.Create(ModuleId.Is("battery"), 0));
        Assert.Equal("""
        capacity ('0') must be a non-negative and non-zero value. (Parameter 'capacity')
        Actual value was 0.
        """, ex.Message);
    }

    [Fact]
    [DocContent(
"""
A Battery with a capacity of 1 has a weight of 3.  
Every 25 extra chapacity after the first 25 adds another 1 weight to the module. 
"""
    )]
    [DocCode(
"""
xychart-beta
    title "Weight Curve"
    x-axis "Capacity" [1, 25, 35, 51, 76, 100]
    y-axis "Weight" 0 --> 6
    bar [3, 3, 4, 5, 6, 6]
""", "mermaid")]
    public void WeightCurve()
    {
        var battery = Battery.Create(ModuleId.Is("battery"), 1);
        Assert.Equal(3, battery.Weight);
        battery = Battery.Create(ModuleId.Is("battery"), 25);
        Assert.Equal(3, battery.Weight);
        battery = Battery.Create(ModuleId.Is("battery"), 35);
        Assert.Equal(4, battery.Weight);
        battery = Battery.Create(ModuleId.Is("battery"), 51);
        Assert.Equal(5, battery.Weight);
        battery = Battery.Create(ModuleId.Is("battery"), 76);
        Assert.Equal(6, battery.Weight);
        battery = Battery.Create(ModuleId.Is("battery"), 100);
        Assert.Equal(6, battery.Weight);
    }
}