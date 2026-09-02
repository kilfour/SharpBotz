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
    [DocExample(typeof(B_DriveTests), nameof(ConstructionExample))]
    [DocContent("TODO.")]
    public void Construction()
    {
        var drive = ConstructionExample();
        Assert.Equal("drive", drive.Id.ToString());
    }

    [CodeSnippet]
    private static Drive ConstructionExample() =>
         Drive.Named("drive")
            .ThrustPerPower(10)
            .MaximumPower(15);

    [Fact]
    public void PowerConsumption()
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
    }
}