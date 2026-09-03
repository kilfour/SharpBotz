using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.F_PowerToSpare;

public class ReserveBot() : Bot(new ReserveBrain(), ModuleRack.Create())
{
    public class ReserveBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
