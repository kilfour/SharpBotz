using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Scanners;
using SharpBotz.Botz.BotModules.Thrusters;

namespace SharpBotz.Challenges.A_DeadAhead;

public class AheadBot() : Bot(nameof(AheadBot), new AheadBrain(),
    ModuleRack.Create(
        Reactor.Named("reactor").MaximumOutput(10),
        Thruster.Named("thruster").ThrustPerPower(20).MaximumPower(3),
        Melee.Named("melee").DamagePerPower(10).MaximumPower(3),
        Scanner.Named("scanner").PowerPerRange(5).MaximumPower(5)
    ))
{
    public class AheadBrain : BotBrain
    {
        protected override PowerPlan RoutePower(ModuleControl modules, BotObservation observation)
        {
            var scan = modules.RequireModule<ScannerInfo>().Scan(1);
            var ahead = observation.Scan[0, 1];
            if (ahead is ScanResult.Bot)
            {
                var melee = modules.RequireModule<MeleeInfo>().Hit(30);
                return PowerPlan.From(
                    modules.RequireModule<ReactorInfo>().SetOutput(melee.Power + scan.Power),
                    scan,
                    melee
                );
            }
            var thruster = modules.RequireModule<ThrusterInfo>().Move(1);
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(thruster.Power + scan.Power),
                scan,
                thruster
            );
        }
    }
}

