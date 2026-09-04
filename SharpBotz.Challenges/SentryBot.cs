using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.RangedWeapons;
using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Challenges;

public class SentryBot(int range, int damage) : Bot(
    nameof(SentryBot),
    new SentryBrain(damage),
    ModuleRack.Create(
        Reactor.Named("reactor").MaximumOutput(1),
        Ranged.Named("ranged")
            .Range(range)
            .DamagePerPower(damage)
            .MaximumPower(1)))
{
    private class SentryBrain(int damage) : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var shot = modules.RequireModule<RangedInfo>().Fire(damage);
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(shot.Power),
                shot);
        }
    }
}
