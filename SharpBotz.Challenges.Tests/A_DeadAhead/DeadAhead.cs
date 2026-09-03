using SharpBotz.Challenges.A_DeadAhead;

namespace SharpBotz.Challenges.Tests.A_DeadAhead;

public class DeadAheadTests
{
    [Fact]
    public void ChallengeCheck()
    {
        var scenario = DeadAhead.Challenge;
        var world = scenario.CreateWorld();
        while (!world.IsComplete)
            world.Update();
        Assert.False(world.GoalReached);
    }
}

