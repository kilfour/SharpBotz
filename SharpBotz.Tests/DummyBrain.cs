using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Tests;

public class DummyBrain : BotBrain
{
    protected override PowerPlan RoutePower(BotObservation observation) =>
        PowerPlan.Empty;
}