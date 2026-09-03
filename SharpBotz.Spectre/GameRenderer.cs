using SharpBotz.Worlds;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace SharpBotz.Spectre;

public static class GameRenderer
{
    public static IRenderable Render(
        GameWorld world,
        string title,
        string speed,
        bool isPaused)
    {
        var state = world.IsComplete
            ? "[bold green]Finished[/]"
            : isPaused
                ? "[bold yellow]Paused[/]"
                : "[bold green]Running[/]";
        var controls = "[grey]| ←/→ speed | Space pause | Enter step[/]";

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
                world.Turn,
                world.MaximumTurns));
    }
}
