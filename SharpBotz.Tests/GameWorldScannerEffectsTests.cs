using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Scanners;
using SharpBotz.Worlds;

namespace SharpBotz.Tests;

public class GameWorldScannerEffectsTests
{
    [Fact]
    public void PoweredScannerSuppliesASquareScanOnTheFollowingTurn()
    {
        var brain = new ScanningBrain(range: 2);
        var world = CreateWorld(
            CreateArena(width: 5),
            CreateScannerState(brain, 1, 1, Direction.Right, maximumPower: 2),
            CreateIdleState(2, 1, Direction.Left));

        world.Update();
        world.Update();

        Assert.Equal(2, brain.Scans.Count);
        Assert.Equal(1, brain.Scans[0].GetLength(0));

        var scan = brain.Scans[1];
        Assert.Equal(5, scan.GetLength(0));
        Assert.Equal(5, scan.GetLength(1));
        Assert.Equal(new ScanResult.OwnBot(Direction.Right, 100), scan[2, 2]);
        Assert.Equal(new ScanResult.Bot(Direction.Left, 100), scan[3, 2]);
        Assert.IsType<ScanResult.Wall>(scan[1, 1]);
        Assert.IsType<ScanResult.Empty>(scan[3, 3]);
        Assert.IsType<ScanResult.OutOfBounds>(scan[0, 0]);
    }

    [Fact]
    public void MultipleScannersUseTheGreatestRange()
    {
        var brain = new MultipleScannerBrain();
        var bot = new Bot(
            brain,
            ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(3),
                Battery.Named("battery").Capacity(10),
                Scanner.Named("short-scanner")
                    .PowerPerRange(1)
                    .MaximumPower(1),
                Scanner.Named("long-scanner")
                    .PowerPerRange(1)
                    .MaximumPower(2)));
        var world = CreateWorld(
            CreateArena(width: 7),
            new BotState(bot, new Position(3, 2), Direction.Up));

        world.Update();
        world.Update();

        Assert.Equal(5, brain.Scans[1].GetLength(0));
        Assert.Equal(5, brain.Scans[1].GetLength(1));
    }

    [Fact]
    public void ScanReflectsMovementFromThePreviousTurn()
    {
        var brain = new ScanningBrain(range: 3);
        var world = CreateWorld(
            CreateArena(width: 7),
            CreateScannerState(brain, 1, 2, Direction.Right, maximumPower: 3),
            CreateMovingState(4, 2, Direction.Left));

        world.Update();
        world.Update();

        Assert.Equal(new ScanResult.Bot(Direction.Left, 100), brain.Scans[1][5, 3]);
        Assert.IsType<ScanResult.Empty>(brain.Scans[1][6, 3]);
    }

    [Fact]
    public void OverchargedScannerStillScansAndDamagesItsBot()
    {
        var brain = new OneShotScanningBrain(range: 2);
        var world = CreateWorld(
            CreateArena(width: 5),
            new BotState(
                new Bot(
                    brain,
                    ModuleRack.Create(
                        Reactor.Named("reactor").MaximumOutput(2),
                        Battery.Named("battery").Capacity(10),
                        Scanner.Named("scanner")
                            .PowerPerRange(1)
                            .MaximumPower(1))),
                new Position(2, 2),
                Direction.Up));

        world.Update();
        world.Update();

        Assert.Equal(97, world.Bots[0].Bot.HitPoints);
        Assert.Equal(5, brain.Scans[1].GetLength(0));
        Assert.Equal(5, brain.Scans[1].GetLength(1));
        Assert.Equal(
            new ScanResult.OwnBot(Direction.Up, HitPoints: 97),
            brain.Scans[1][2, 2]);
    }

    private static Arena CreateArena(int width) =>
        Arena.Sized(
                ArenaWidth.Is(width),
                ArenaHeight.Is(5))
            .Build();

    private static GameWorld CreateWorld(
        Arena arena,
        params BotState[] botStates) =>
        new(
            arena,
            botStates,
            maximumTurns: 10,
            complete: _ => false,
            seed: 1234);

    private static BotState CreateScannerState(
        BotBrain brain,
        int x,
        int y,
        Direction facing,
        int maximumPower) =>
        new(
            new Bot(
                brain,
                ModuleRack.Create(
                    Reactor.Named("reactor").MaximumOutput(maximumPower),
                    Battery.Named("battery").Capacity(10),
                    Scanner.Named("scanner")
                        .PowerPerRange(1)
                        .MaximumPower(maximumPower))),
            new Position(x, y),
            facing);

    private static BotState CreateIdleState(
        int x,
        int y,
        Direction facing) =>
        new(
            new Bot(
                new IdleBrain(),
                ModuleRack.Create(Battery.Named("battery").Capacity(10))),
            new Position(x, y),
            facing);

    private static BotState CreateMovingState(
        int x,
        int y,
        Direction facing) =>
        new(
            new Bot(
                new MovingBrain(),
                ModuleRack.Create(
                    Reactor.Named("reactor").MaximumOutput(1),
                    Battery.Named("battery").Capacity(10),
                    Drive.Named("drive")
                        .ThrustPerPower(100)
                        .MaximumPower(1))),
            new Position(x, y),
            facing);

    private class ScanningBrain(int range) : BotBrain
    {
        public List<ScanResult[,]> Scans { get; } = [];

        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            Scans.Add(observation.Scan);
            var scan = modules.RequireModule<ScannerInfo>().Scan(range);
            return new(
                modules.RequireModule<ReactorInfo>().SetOutput(scan.Power),
                scan);
        }
    }

    private class MultipleScannerBrain : BotBrain
    {
        public List<ScanResult[,]> Scans { get; } = [];

        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            Scans.Add(observation.Scan);
            var scans = modules.FindModules<ScannerInfo>()
                .Select(scanner => scanner.Scan(scanner.MaximumRange))
                .ToArray();
            return new([
                modules.RequireModule<ReactorInfo>()
                    .SetOutput(scans.Sum(scan => scan.Power)),
                .. scans,
            ]);
        }
    }

    private class OneShotScanningBrain(int range) : BotBrain
    {
        public List<ScanResult[,]> Scans { get; } = [];

        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            Scans.Add(observation.Scan);
            if (Scans.Count > 1)
            {
                return PowerPlan.Empty;
            }

            var scan = modules.RequireModule<ScannerInfo>().Scan(range);
            return new(
                modules.RequireModule<ReactorInfo>().SetOutput(scan.Power),
                scan);
        }
    }

    private class MovingBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var movement = modules.RequireModule<DrivingInfo>().Move(1);
            return new(
                modules.RequireModule<ReactorInfo>().SetOutput(movement.Power),
                movement);
        }
    }

    private class IdleBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
