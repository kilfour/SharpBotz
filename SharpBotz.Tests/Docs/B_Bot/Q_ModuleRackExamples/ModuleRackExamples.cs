using QuickPulse.Explains;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Scanners;

namespace SharpBotz.Tests.Docs.B_Bot.Q_ModuleRackExamples;

[DocFile]
[DocFileHeader("Example Module Racks")]
public class ModuleRackExamples
{
    [Fact]
    [DocContent(
    """
    Every module rack includes a chassis weighing 10, even when no modules are installed:
    """)]
    [DocExample(typeof(ModuleRackExamples), nameof(ChassisOnly))]
    [DocContent("This rack weighs **10**.")]
    public void ChassisOnlyWeight()
    {
        Assert.Equal(10, ChassisOnly().TotalWeight);
    }

    [CodeSnippet]
    public static ModuleRack ChassisOnly() =>
        ModuleRack.Create();

    [Fact]
    [DocContent(
    """
    A small mobile rack combines a one-power reactor with a drive.
    Its high thrust efficiency lets it move the loaded chassis using that single unit of power:
    """)]
    [DocExample(typeof(ModuleRackExamples), nameof(Mobile))]
    [DocContent("This rack weighs **63**.")]
    public void MobileWeight()
    {
        Assert.Equal(63, Mobile().TotalWeight);
    }

    [CodeSnippet]
    public static ModuleRack Mobile() =>
        ModuleRack.Create(
            Reactor.Named("reactor").MaximumOutput(1),
            Drive.Named("drive")
                .ThrustPerPower(100)
                .MaximumPower(1));

    [Fact]
    [DocContent(
    """
    A close-combat rack can move, scan its surroundings, and strike an adjacent bot.
    Its battery stores unused reactor output for later turns:
    """)]
    [DocExample(typeof(ModuleRackExamples), nameof(CloseCombat))]
    [DocContent("This rack weighs **80**, leaving 20 weight available for future upgrades.")]
    public void CloseCombatWeight()
    {
        Assert.Equal(80, CloseCombat().TotalWeight);
    }

    [CodeSnippet]
    public static ModuleRack CloseCombat() =>
        ModuleRack.Create(
            Reactor.Named("reactor").MaximumOutput(3),
            Battery.Named("battery").Capacity(10),
            Drive.Named("drive")
                .ThrustPerPower(100)
                .MaximumPower(1),
            Melee.Named("melee")
                .DamagePerPower(20)
                .MaximumPower(1),
            Scanner.Named("scanner")
                .PowerPerRange(1)
                .MaximumPower(1));
}
