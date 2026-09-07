using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Scanners;

namespace SharpBotz.Challenges.A_DeadAhead;

public class AheadBot() : Bot(nameof(AheadBot), new AheadBrain(),
    ModuleRack.Create(
        Reactor.Named("reactor").MaximumOutput(12),
        Drive.Named("drive").ThrustPerPower(11).MaximumPower(8),
        Melee.Named("melee").DamagePerPower(20).MaximumPower(5),
        Scanner.Named("scanner").PowerPerRange(3).MaximumPower(3)
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
                var melee = modules.RequireModule<MeleeInfo>().Hit(100);
                return PowerPlan.From(
                    modules.RequireModule<ReactorInfo>().SetOutput(melee.Power + scan.Power),
                    scan,
                    melee
                );
            }
            var drive = modules.RequireModule<DrivingInfo>().Move(1);
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(drive.Power + scan.Power),
                scan,
                drive
            );
        }
    }
}

