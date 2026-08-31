using QuickPulse.Explains;
using SharpBotz.Botz.BotModules.Drives;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.C_PoweredModules;

[DocFile]
public class B_DriveTests
{
    [Fact]
    [DocContent(
"""
A drive is needed in order to move your board across the arena.


It is created by passing in it's thrustPerPower and maximumPower along with a ModuleId (supplied as string).
""")]
    // [DocExample(typeof(B_DriveTests), nameof(ConstructionExample))]
    [DocContent("It's initial Charge is set to zero.")]
    public void Construction()
    {
        var drive = ConstructionExample();
        var info = (DrivingInfo)drive.GetInfo(50);
        var allocation = info.Move(1);
        Assert.Equal(5, allocation.Power);
        allocation = info.Move(2);
        Assert.Equal(10, allocation.Power);
        allocation = info.Move(3);
        Assert.Equal(15, allocation.Power);
        allocation = info.Move(4);
        Assert.Equal(20, allocation.Power);
        // Assert.Equal(100, drive.Capacity);
        // Assert.Equal(100, drive.AvailableCapacity);
        // Assert.Equal(0, drive.Charge);
    }

    [CodeSnippet]
    private static Drive ConstructionExample() =>
         // bot weighs 50, so speed 1 => 5 power, 2 => 10 Power
         Drive.Named("drive")
            .ThrustPerPower(10)
            .MaximumPower(15);
}