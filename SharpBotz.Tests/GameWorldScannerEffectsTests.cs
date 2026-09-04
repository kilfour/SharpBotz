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
        Assert.Equal(1, brain.Scans[0].Size);

        var scan = brain.Scans[1];
        Assert.Equal(2, scan.Range);
        Assert.Equal(5, scan.Size);
        Assert.Equal(new ScanResult.OwnBot(Direction.Right, 100), scan[0, 0]);
        Assert.Equal(new ScanResult.Bot(Direction.Left, 100), scan[0, 1]);
        Assert.IsType<ScanResult.Wall>(scan[-1, -1]);
        Assert.IsType<ScanResult.Empty>(scan[1, 0]);
        Assert.IsType<ScanResult.OutOfBounds>(scan[-2, -2]);
    }

    [Theory]
    [InlineData(Direction.Up)]
    [InlineData(Direction.Right)]
    [InlineData(Direction.Down)]
    [InlineData(Direction.Left)]
    public void DirectlyAheadIsAlwaysOnPositiveY(Direction facing)
    {
        var brain = new ScanningBrain(range: 1);
        var observerPosition = new Position(2, 2);
        var targetPosition = observerPosition.Move(facing);
        var world = CreateWorld(
            CreateArena(width: 5),
            CreateScannerState(
                brain,
                observerPosition.X,
                observerPosition.Y,
                facing,
                maximumPower: 1),
            CreateIdleState(
                targetPosition.X,
                targetPosition.Y,
                Direction.Up));

        world.Update();
        world.Update();

        Assert.Equal(
            new ScanResult.Bot(Direction.Up, HitPoints: 100),
            brain.Scans[1][0, 1]);
    }

    [Fact]
    public void MultipleScannersUseTheGreatestRange()
    {
        var brain = new MultipleScannerBrain();
        var bot = Bot.Named("multi-scanner")
            .Brain(brain)
            .Rack(ModuleRack.Create(
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

        Assert.Equal(2, brain.Scans[1].Range);
        Assert.Equal(5, brain.Scans[1].Size);
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

        Assert.Equal(new ScanResult.Bot(Direction.Left, 100), brain.Scans[1][0, 2]);
        Assert.IsType<ScanResult.Empty>(brain.Scans[1][0, 3]);
    }

    [Fact]
    public void OverchargedScannerStillScansAndDamagesItsBot()
    {
        var brain = new OneShotScanningBrain(range: 2);
        var world = CreateWorld(
            CreateArena(width: 5),
            new BotState(
                Bot.Named("overcharged-scanner")
                    .Brain(brain)
                    .Rack(ModuleRack.Create(
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
        Assert.Equal(2, brain.Scans[1].Range);
        Assert.Equal(5, brain.Scans[1].Size);
        Assert.Equal(
            new ScanResult.OwnBot(Direction.Up, HitPoints: 97),
            brain.Scans[1][0, 0]);
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
            Bot.Named($"scanner-{x}-{y}")
                .Brain(brain)
                .Rack(ModuleRack.Create(
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
            Bot.Named($"idle-{x}-{y}")
                .Brain(new IdleBrain())
                .Rack(ModuleRack.Create(Battery.Named("battery").Capacity(10))),
            new Position(x, y),
            facing);

    private static BotState CreateMovingState(
        int x,
        int y,
        Direction facing) =>
        new(
            Bot.Named($"moving-{x}-{y}")
                .Brain(new MovingBrain())
                .Rack(ModuleRack.Create(
                    Reactor.Named("reactor").MaximumOutput(1),
                    Battery.Named("battery").Capacity(10),
                    Drive.Named("drive")
                        .ThrustPerPower(100)
                        .MaximumPower(1))),
            new Position(x, y),
            facing);

    private class ScanningBrain(int range) : BotBrain
    {
        public List<BotScan> Scans { get; } = [];

        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            Scans.Add(observation.Scan);
            var scan = modules.RequireModule<ScannerInfo>().Scan(range);
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(scan.Power),
                scan);
        }
    }

    private class MultipleScannerBrain : BotBrain
    {
        public List<BotScan> Scans { get; } = [];

        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            Scans.Add(observation.Scan);
            var scans = modules.FindModules<ScannerInfo>()
                .Select(scanner => scanner.Scan(scanner.MaximumRange))
                .ToArray();
            return PowerPlan.From([
                modules.RequireModule<ReactorInfo>()
                    .SetOutput(scans.Sum(scan => scan.Power)),
                .. scans,
            ]);
        }
    }

    private class OneShotScanningBrain(int range) : BotBrain
    {
        public List<BotScan> Scans { get; } = [];

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
            return PowerPlan.From(
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
            return PowerPlan.From(
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
