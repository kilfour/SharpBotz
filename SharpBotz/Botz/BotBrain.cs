using QuickFuzzr;
using QuickFuzzr.UnderTheHood;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Botz;

public abstract class BotBrain
{
    private State? fuzzrState;

    protected T Generate<T>(FuzzrOf<T> fuzzr)
    {
        ArgumentNullException.ThrowIfNull(fuzzr);
        var state = fuzzrState ?? throw new InvalidOperationException(
            "Random values can only be generated while the bot is deciding its power plan.");
        return fuzzr(state).Value;
    }

    public PowerPlan Decide(BotObservation observation, State state)
    {
        ArgumentNullException.ThrowIfNull(state);
        if (fuzzrState is not null)
        {
            throw new InvalidOperationException(
                $"Bot cannot make overlapping decisions.");
        }

        fuzzrState = state;
        try
        {
            return RoutePower(observation) ?? throw new InvalidOperationException(
                $"Bot returned a null power plan.");
        }
        finally
        {
            fuzzrState = null;
        }
    }

    protected abstract PowerPlan RoutePower(BotObservation observation);
}
