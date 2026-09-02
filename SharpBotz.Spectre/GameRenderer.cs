using SharpBotz.Worlds;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace SharpBotz.Spectre;

public static class GameRenderer
{
    public static IRenderable Render(
        GameWorld world,
        string title,
        int turns,
        int maximumTurns,
        bool isFinished,
        string speed,
        bool isPaused,
        bool controlsAvailable)
    {
        var state = isFinished
            ? "[bold green]Finished[/]"
            : isPaused
                ? "[bold yellow]Paused[/]"
                : "[bold green]Running[/]";
        var controls = controlsAvailable
            ? "[grey]| ←/→ speed | Space pause | Enter step[/]"
            : "[grey]| controls unavailable[/]";

        return new Rows(
            new Panel(ArenaRenderer.Render(world.Arena, world.Bots))
                .Header(
                    $"[bold yellow]SharpBotz Arena · {Markup.Escape(title)} · " +
                    $"Seed {world.Seed}[/]")
                .Border(BoxBorder.Rounded)
                .Padding(1, 0),
            BotTableRenderer.Render(
                world.Bots,
                state,
                speed,
                controls,
                turns,
                maximumTurns));
    }
}
