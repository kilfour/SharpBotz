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
using SharpBotz.Worlds;

namespace SharpBotz.Challenges.Tests;

public class ChallengeTests
{
    [Fact(Skip = "Not Implemented")]
    public void DeadAhead_Challenge() =>
        Assert.True(RunScenario(DeadAhead.Challenge).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void DifferentRoutes_RouteOne() =>
        Assert.True(RunScenario(DifferentRoutes.RouteOne).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void DifferentRoutes_RouteTwo() =>
        Assert.True(RunScenario(DifferentRoutes.RouteTwo).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void LongShot_TargetEast() =>
        Assert.True(RunScenario(LongShot.TargetEast).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void LongShot_TargetSouth() =>
        Assert.True(RunScenario(LongShot.TargetSouth).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void LongShot_TargetWest() =>
        Assert.True(RunScenario(LongShot.TargetWest).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void CleanSweep_CardinalPoints() =>
        Assert.True(RunScenario(CleanSweep.CardinalPoints).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void CleanSweep_FourCorners() =>
        Assert.True(RunScenario(CleanSweep.FourCorners).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void BehindCover_LowerPassage() =>
        Assert.True(RunScenario(BehindCover.LowerPassage).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void BehindCover_UpperPassage() =>
        Assert.True(RunScenario(BehindCover.UpperPassage).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void PowerToSpare_FiveInReserve() =>
        Assert.True(RunScenario(PowerToSpare.FiveInReserve).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void PowerToSpare_TenInReserve() =>
        Assert.True(RunScenario(PowerToSpare.TenInReserve).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void UnderFire_FireFromTheEast() =>
        Assert.True(RunScenario(UnderFire.FireFromTheEast).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void UnderFire_FireFromTheNorth() =>
        Assert.True(RunScenario(UnderFire.FireFromTheNorth).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void Crossfire_ThreeWays() =>
        Assert.True(RunScenario(Crossfire.ThreeWays).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void Crossfire_FourWays() =>
        Assert.True(RunScenario(Crossfire.FourWays).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void MovingTargets_SinglePatrol() =>
        Assert.True(RunScenario(MovingTargets.SinglePatrol).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void MovingTargets_TwoPatrols() =>
        Assert.True(RunScenario(MovingTargets.TwoPatrols).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void TrialByFire_OpenArena() =>
        Assert.True(RunScenario(TrialByFire.OpenArena).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void TrialByFire_BrokenGround() =>
        Assert.True(RunScenario(TrialByFire.BrokenGround).GoalReached);

    [Fact(Skip = "Not Implemented")]
    public void TrialByFire_FinalArena() =>
        Assert.True(RunScenario(TrialByFire.FinalArena).GoalReached);

    [Fact]
    public void EveryScenarioRunsToCompletion()
    {
        foreach (var scenario in AllScenarios())
        {
            var world = RunScenario(scenario);
            Assert.True(world.IsComplete, scenario.Name);
        }
    }

    private static IEnumerable<Scenario> AllScenarios()
    {
        yield return DeadAhead.Challenge;
        yield return DifferentRoutes.RouteOne;
        yield return DifferentRoutes.RouteTwo;
        yield return LongShot.TargetEast;
        yield return LongShot.TargetSouth;
        yield return LongShot.TargetWest;
        yield return CleanSweep.CardinalPoints;
        yield return CleanSweep.FourCorners;
        yield return BehindCover.LowerPassage;
        yield return BehindCover.UpperPassage;
        yield return PowerToSpare.FiveInReserve;
        yield return PowerToSpare.TenInReserve;
        yield return UnderFire.FireFromTheEast;
        yield return UnderFire.FireFromTheNorth;
        yield return Crossfire.ThreeWays;
        yield return Crossfire.FourWays;
        yield return MovingTargets.SinglePatrol;
        yield return MovingTargets.TwoPatrols;
        yield return TrialByFire.OpenArena;
        yield return TrialByFire.BrokenGround;
        yield return TrialByFire.FinalArena;
    }

    private static GameWorld RunScenario(Scenario scenario)
    {
        var world = scenario.CreateWorld();
        while (!world.IsComplete)
            world.Update();
        return world;
    }
}

