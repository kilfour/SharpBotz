using SharpBotz.Challenges.A_DeadAhead;
using SharpBotz.Challenges.B_DifferentRoutes;
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

    private static GameWorld RunScenario(Scenario scenario)
    {
        var world = scenario.CreateWorld();
        while (!world.IsComplete)
            world.Update();
        return world;
    }
}

