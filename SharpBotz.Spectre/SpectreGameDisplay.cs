using SharpBotz.Worlds;
using Spectre.Console;
using System.Diagnostics;

namespace SharpBotz.Spectre;

public class SpectreGameDisplay
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

    private int speedIndex = 3;

    private async Task WaitForNextTurnAsync(
        GameWorld world,
        string title,
        LiveDisplayContext context,
        DisplayState state,
        CancellationToken cancellationToken)
    {
        var clock = Stopwatch.StartNew();
        var nextUpdate = clock.Elapsed + CurrentSpeed.Interval;
        while (true)
        {
            var controls = ReadControls(state);
            if (controls.ResetUpdateTimer)
            {
                nextUpdate = clock.Elapsed + CurrentSpeed.Interval;
            }
            if (controls.ShouldRender)
            {
                Refresh(context, world, title, state.IsPaused);
            }
            if (controls.AdvanceOneTurn ||
                !state.IsPaused && clock.Elapsed >= nextUpdate)
            {
                return;
            }

            await Task.Delay(20, cancellationToken);
        }
    }

    public async Task<int> RunAsync(
        GameWorld world,
        string title,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        AnsiConsole.Clear();
        await AnsiConsole
            .Live(new Markup("[grey]Starting scenario...[/]"))
            .AutoClear(false)
            .StartAsync(async context =>
            {
                var state = new DisplayState();
                Refresh(context, world, title, state.IsPaused);
                while (!world.IsComplete)
                {
                    await WaitForNextTurnAsync(
                        world,
                        title,
                        context,
                        state,
                        cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    world.Update();
                    Refresh(context, world, title, state.IsPaused);
                }
            });
        return world.Turn;
    }

    private void Refresh(
        LiveDisplayContext context,
        GameWorld world,
        string title,
        bool isPaused)
    {
        context.UpdateTarget(GameRenderer.Render(
            world,
            title,
            CurrentSpeed.Label,
            isPaused));
        context.Refresh();
    }

    private ControlUpdate ReadControls(DisplayState state)
    {
        var update = new ControlUpdate();
        var input = AnsiConsole.Console.Input;
        while (input.IsKeyAvailable())
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
                    state.IsPaused = !state.IsPaused;
                    update = new(ShouldRender: true, ResetUpdateTimer: true);
                    break;
                case ConsoleKey.Enter when state.IsPaused:
                    update = update with { AdvanceOneTurn = true };
                    break;
            }
        }

        return update;
    }

    private SimulationSpeed CurrentSpeed => speeds[speedIndex];

    private readonly record struct SimulationSpeed(string Label, TimeSpan Interval);

    private sealed class DisplayState
    {
        public bool IsPaused { get; set; }
    }

    private readonly record struct ControlUpdate(
        bool ShouldRender = false,
        bool ResetUpdateTimer = false,
        bool AdvanceOneTurn = false);
}
