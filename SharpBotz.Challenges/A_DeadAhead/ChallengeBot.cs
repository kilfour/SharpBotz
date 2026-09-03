using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.A_DeadAhead;

public class ChallengeBot() : Bot(new ChallengeBrain(), ModuleRack.Create())
{
    public class ChallengeBrain : BotBrain
    {
        protected override PowerPlan RoutePower(ModuleControl modules, BotObservation observation) =>
            PowerPlan.Empty;
    }
}

