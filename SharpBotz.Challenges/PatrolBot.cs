using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Rotators;

namespace SharpBotz.Challenges;

public class PatrolBot() : Bot(
    nameof(PatrolBot),
    new PatrolBrain(),
    ModuleRack.Create(
        Reactor.Named("reactor").MaximumOutput(4),
        Drive.Named("drive")
            .ThrustPerPower(50)
            .MaximumPower(2),
        Rotator.Named("rotator")
            .TorquePerPower(50)
            .MaximumPower(2)
            .Right()))
{
    private class PatrolBrain : BotBrain
    {
        private int turn;

        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            turn++;
            var movement = modules.RequireModule<DrivingInfo>().Move(1);
            if (turn % 4 != 0)
            {
                return PowerPlan.From(
                    modules.RequireModule<ReactorInfo>().SetOutput(movement.Power),
                    movement);
            }

            var rotator = modules.RequireModule<RightRotatorInfo>();
            var rotation = new PowerAllocation(rotator.Id, Power: 2);
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>()
                    .SetOutput(movement.Power + rotation.Power),
                movement,
                rotation);
        }
    }
}
