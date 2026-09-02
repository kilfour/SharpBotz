using SharpBotz.Worlds;
using Spectre.Console;
using System.Diagnostics;

namespace SharpBotz.Spectre;

public delegate Task GameWorldRenderer(
    GameWorld world,
    int turns,
    int maximumTurns,
    bool isFinished,
    CancellationToken cancellationToken);

public sealed class SpectreGameDisplay
{
    private readonly SimulationSpeed[] speeds =
    [
        new("0.25x", TimeSpan.FromMilliseconds(2000)),
        new("0.5x", TimeSpan.FromMilliseconds(1000)),
        new("1x", TimeSpan.FromMilliseconds(500)),
        new("2x", TimeSpan.FromMilliseconds(250)),
        new("4x", TimeSpan.FromMilliseconds(125)),
        new("10x", TimeSpan.FromMilliseconds(50)),
        new("20x", TimeSpan.FromMilliseconds(25)),
        new("50x", TimeSpan.FromMilliseconds(5)),
    ];

    private readonly bool controlsAvailable =
        AnsiConsole.Profile.Capabilities.Interactive;
    private int speedIndex = 3;

    public GameWorldRenderer CreateRenderer(string title, LiveDisplayContext context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(context);
        var isPaused = false;

        return async (world, turns, maximumTurns, isFinished, cancellationToken) =>
        {
            Refresh(
                context,
                world,
                title,
                turns,
                maximumTurns,
                isFinished,
                isPaused);
            if (isFinished)
            {
                return;
            }

            var clock = Stopwatch.StartNew();
            var nextUpdate = clock.Elapsed + CurrentSpeed.Interval;
            while (true)
            {
                var controls = ReadControls(ref isPaused);
                if (controls.ResetUpdateTimer)
                {
                    nextUpdate = clock.Elapsed + CurrentSpeed.Interval;
                }
                if (controls.ShouldRender)
                {
                    Refresh(
                        context,
                        world,
                        title,
                        turns,
                        maximumTurns,
                        isFinished: false,
                        isPaused);
                }
                if (controls.AdvanceOneTurn ||
                    !isPaused && clock.Elapsed >= nextUpdate)
                {
                    return;
                }

                await Task.Delay(20, cancellationToken);
            }
        };
    }

    public async Task<int> RunAsync(
        GameWorld world,
        string title,
        int maximumTurns,
        Func<GameWorld, bool>? isComplete = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumTurns);
        isComplete ??= currentWorld =>
            currentWorld.Bots.Count(bot => bot.Bot.IsAlive) <= 1;

        if (!controlsAvailable)
        {
            return await RunNonInteractiveAsync(
                world,
                title,
                maximumTurns,
                isComplete,
                cancellationToken);
        }

        var turns = 0;
        AnsiConsole.Clear();
        await AnsiConsole
            .Live(new Markup("[grey]Starting scenario...[/]"))
            .AutoClear(false)
            .StartAsync(async context =>
            {
                var renderer = CreateRenderer(title, context);
                var isFinished = isComplete(world) || turns >= maximumTurns;
                await renderer(
                    world,
                    turns,
                    maximumTurns,
                    isFinished,
                    cancellationToken);

                while (!isFinished && turns < maximumTurns)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    world.Update();
                    turns++;
                    isFinished = isComplete(world) || turns >= maximumTurns;
                    await renderer(
                        world,
                        turns,
                        maximumTurns,
                        isFinished,
                        cancellationToken);
                }
            });
        return turns;
    }

    private async Task<int> RunNonInteractiveAsync(
        GameWorld world,
        string title,
        int maximumTurns,
        Func<GameWorld, bool> isComplete,
        CancellationToken cancellationToken)
    {
        var turns = 0;
        var isFinished = isComplete(world) || turns >= maximumTurns;
        RenderNonInteractiveFrame(
            world,
            title,
            turns,
            maximumTurns,
            isFinished);

        while (!isFinished && turns < maximumTurns)
        {
            await Task.Delay(CurrentSpeed.Interval, cancellationToken);
            world.Update();
            turns++;
            isFinished = isComplete(world) || turns >= maximumTurns;
            RenderNonInteractiveFrame(
                world,
                title,
                turns,
                maximumTurns,
                isFinished);
        }

        return turns;
    }

    private void RenderNonInteractiveFrame(
        GameWorld world,
        string title,
        int turns,
        int maximumTurns,
        bool isFinished)
    {
        AnsiConsole.Write(GameRenderer.Render(
            world,
            title,
            turns,
            maximumTurns,
            isFinished,
            CurrentSpeed.Label,
            isPaused: false,
            controlsAvailable: false));
        AnsiConsole.WriteLine();
    }

    private void Refresh(
        LiveDisplayContext context,
        GameWorld world,
        string title,
        int turns,
        int maximumTurns,
        bool isFinished,
        bool isPaused)
    {
        context.UpdateTarget(GameRenderer.Render(
            world,
            title,
            turns,
            maximumTurns,
            isFinished,
            CurrentSpeed.Label,
            isPaused,
            controlsAvailable));
        context.Refresh();
    }

    private ControlUpdate ReadControls(ref bool isPaused)
    {
        var update = new ControlUpdate();
        var input = AnsiConsole.Console.Input;
        while (controlsAvailable && input.IsKeyAvailable())
        {
            var key = input.ReadKey(intercept: true);
            if (key is not { } pressedKey)
            {
                continue;
            }

            switch (pressedKey.Key)
            {
                case ConsoleKey.OemPlus:
                case ConsoleKey.Add:
                case ConsoleKey.RightArrow:
                    if (speedIndex < speeds.Length - 1)
                    {
                        speedIndex++;
                        update = new(ShouldRender: true, ResetUpdateTimer: true);
                    }
                    break;
                case ConsoleKey.OemMinus:
                case ConsoleKey.Subtract:
                case ConsoleKey.LeftArrow:
                    if (speedIndex > 0)
                    {
                        speedIndex--;
                        update = new(ShouldRender: true, ResetUpdateTimer: true);
                    }
                    break;
                case ConsoleKey.Spacebar:
                    isPaused = !isPaused;
                    update = new(ShouldRender: true, ResetUpdateTimer: true);
                    break;
                case ConsoleKey.Enter when isPaused:
                    update = update with { AdvanceOneTurn = true };
                    break;
            }
        }

        return update;
    }

    private SimulationSpeed CurrentSpeed => speeds[speedIndex];

    private readonly record struct SimulationSpeed(string Label, TimeSpan Interval);

    private readonly record struct ControlUpdate(
        bool ShouldRender = false,
        bool ResetUpdateTimer = false,
        bool AdvanceOneTurn = false);
}
