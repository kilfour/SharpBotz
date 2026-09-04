using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules.Scanners;

namespace SharpBotz.Worlds.EffectResolving;

public static class ScannerEffectsResolver
{
    private static readonly ScanResult Empty = new ScanResult.Empty();
    private static readonly ScanResult Wall = new ScanResult.Wall();
    private static readonly ScanResult OutOfBounds = new ScanResult.OutOfBounds();

    public static BotObservation[] Observe(Arena arena, BotState[] botStates) =>
        [.. botStates.Select(botState => CreateObservation(
            arena,
            botState,
            botStates,
            range: 0))];

    public static BotObservation[] Handle(
        Arena arena,
        BotStateEffect[] botStateEffects)
    {
        var botStates = botStateEffects
            .Select(botStateEffect => botStateEffect.BotState)
            .ToArray();

        foreach (var botStateEffect in botStateEffects)
        {
            foreach (var effect in botStateEffect.Effects.ScannerEffects.OfType<ScannerOverChargedEffect>())
            {
                botStateEffect.BotState.Bot.TakeDamage(effect.ExcessPower * 3);
            }
        }

        return [.. botStateEffects.Select(botStateEffect =>
        {
            var range = botStateEffect.Effects.ScannerEffects
                .OfType<ScanEffect>()
                .Select(effect => effect.Range)
                .DefaultIfEmpty()
                .Max();
            return CreateObservation(
                arena,
                botStateEffect.BotState,
                botStates,
                range);
        })];
    }

    private static BotObservation CreateObservation(
        Arena arena,
        BotState observer,
        BotState[] botStates,
        int range)
    {
        var size = checked((range * 2) + 1);
        var scan = new ScanResult[size, size];
        var livingBots = botStates
            .Where(botState => botState.Bot.IsAlive)
            .ToLookup(botState => botState.Position.ToCoordinates());

        for (var scanX = 0; scanX < size; scanX++)
        {
            for (var scanY = 0; scanY < size; scanY++)
            {
                if (scanX == range && scanY == range)
                {
                    scan[scanX, scanY] = new ScanResult.OwnBot(
                        observer.Facing,
                        observer.Bot.HitPoints);
                    continue;
                }

                var relativeX = scanX - range;
                var relativeY = scanY - range;
                var (arenaX, arenaY) = ToArenaCoordinates(
                    observer,
                    relativeX,
                    relativeY);
                scan[scanX, scanY] = CreateScanResult(
                    arena,
                    arenaX,
                    arenaY,
                    livingBots[(arenaX, arenaY)].LastOrDefault());
            }
        }

        return new(scan);
    }

    private static (int X, int Y) ToArenaCoordinates(
        BotState observer,
        int relativeX,
        int relativeY)
    {
        var (arenaOffsetX, arenaOffsetY) = observer.Facing switch
        {
            Direction.Up => (relativeX, relativeY),
            Direction.Right => (-relativeY, relativeX),
            Direction.Down => (-relativeX, -relativeY),
            Direction.Left => (relativeY, -relativeX),
            _ => throw new ArgumentOutOfRangeException(
                nameof(observer.Facing),
                observer.Facing,
                "Unknown bot direction."),
        };

        return (
            checked(observer.Position.X + arenaOffsetX),
            checked(observer.Position.Y + arenaOffsetY));
    }

    private static ScanResult CreateScanResult(
        Arena arena,
        int x,
        int y,
        BotState? scannedBot)
    {
        if (x < 0 || x >= arena.Width || y < 0 || y >= arena.Height)
        {
            return OutOfBounds;
        }

        if (arena[x, y] == ArenaTileType.Wall)
        {
            return Wall;
        }

        return scannedBot is null
            ? Empty
            : new ScanResult.Bot(scannedBot.Facing, scannedBot.Bot.HitPoints);
    }
}
