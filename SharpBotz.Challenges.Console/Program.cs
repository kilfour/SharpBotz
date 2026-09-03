
using SharpBotz.Challenges.A_DeadAhead;
using SharpBotz.Challenges.B_DifferentRoutes;
using SharpBotz.Challenges.C_LongShot;
using SharpBotz.Challenges.D_CleanSweep;
using SharpBotz.Challenges.E_BehindCover;
using SharpBotz.Challenges.F_PowerToSpare;
using SharpBotz.Challenges.G_UnderFire;
using SharpBotz.Challenges.H_Crossfire;
using SharpBotz.Challenges.I_MovingTargets;
using SharpBotz.Challenges.J_TrialByFire;
using SharpBotz.Scenarios;
using SharpBotz.Spectre;
using Spectre.Console;

var scenarios = new ScenarioOption[]
{
    new("A. Dead Ahead", () => DeadAhead.Challenge),
    new("B. Different Routes - Route One", () => DifferentRoutes.RouteOne),
    new("B. Different Routes - Route Two", () => DifferentRoutes.RouteTwo),
    new("C. Long Shot - Target East", () => LongShot.TargetEast),
    new("C. Long Shot - Target South", () => LongShot.TargetSouth),
    new("C. Long Shot - Target West", () => LongShot.TargetWest),
    new("D. Clean Sweep - Cardinal Points", () => CleanSweep.CardinalPoints),
    new("D. Clean Sweep - Four Corners", () => CleanSweep.FourCorners),
    new("E. Behind Cover - Lower Passage", () => BehindCover.LowerPassage),
    new("E. Behind Cover - Upper Passage", () => BehindCover.UpperPassage),
    new("F. Power To Spare - Five In Reserve", () => PowerToSpare.FiveInReserve),
    new("F. Power To Spare - Ten In Reserve", () => PowerToSpare.TenInReserve),
    new("G. Under Fire - From The East", () => UnderFire.FireFromTheEast),
    new("G. Under Fire - From The North", () => UnderFire.FireFromTheNorth),
    new("H. Crossfire - Three Ways", () => Crossfire.ThreeWays),
    new("H. Crossfire - Four Ways", () => Crossfire.FourWays),
    new("I. Moving Targets - Single Patrol", () => MovingTargets.SinglePatrol),
    new("I. Moving Targets - Two Patrols", () => MovingTargets.TwoPatrols),
    new("J. Trial By Fire - Open Arena", () => TrialByFire.OpenArena),
    new("J. Trial By Fire - Broken Ground", () => TrialByFire.BrokenGround),
    new("J. Trial By Fire - Final Arena", () => TrialByFire.FinalArena),
};

var selected = AnsiConsole.Prompt(
    new SelectionPrompt<ScenarioOption>()
        .Title("[bold yellow]Choose a SharpBotz scenario[/]")
        .PageSize(12)
        .MoreChoicesText("[grey](Move up and down to see more scenarios)[/]")
        .UseConverter(option => option.Name)
        .AddChoices(scenarios));

var scenario = selected.CreateScenario();
var world = scenario.CreateWorld();
var display = new SpectreGameDisplay();
await display.RunAsync(world, scenario.Name);

internal sealed record ScenarioOption(
    string Name,
    Func<Scenario> CreateScenario);
