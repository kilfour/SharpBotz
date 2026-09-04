using SharpBotz.Worlds;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace SharpBotz.Spectre;

public static class BotTableRenderer
{
    public static IRenderable Render(
        IReadOnlyList<BotState> bots,
        string state,
        string speed,
        string controls,
        int turns,
        int maximumTurns)
    {
        var table = new Table()
            .Border(TableBorder.None)
            .AddColumn("[bold]Bot[/]")
            .AddColumn(new TableColumn("[bold]HP[/]").RightAligned())
            .AddColumn(new TableColumn("[bold]Position[/]").RightAligned())
            .AddColumn("[bold]Facing[/]")
            .AddColumn(new TableColumn("[bold]Battery[/]").RightAligned())
            .AddColumn(new TableColumn("[bold]Reactor[/]").RightAligned())
            .AddColumn(new TableColumn("[bold]Weight[/]").RightAligned())
            .AddColumn("[bold]Status[/]");

        for (var index = 0; index < bots.Count; index++)
        {
            var botState = bots[index];
            var bot = botState.Bot;
            var rack = bot.ModuleRack;
            var (foreground, _) = BotPalette.GetColors(index);
            table.AddRow(
                $"[{foreground}]● {bot.Name}[/]",
                $"[{foreground}]{bot.HitPoints}[/]",
                $"[{foreground}]({botState.Position.X}, {botState.Position.Y})[/]",
                $"[{foreground}]{botState.Facing}[/]",
                $"[{foreground}]{rack.BatteryLevel}/{rack.BatteryCapacity}[/]",
                $"[{foreground}]{rack.MaximumReactorOutput} max[/]",
                $"[{foreground}]{rack.TotalWeight}[/]",
                bot.IsAlive
                    ? $"[{foreground}]Active[/]"
                    : "[grey]Destroyed[/]");
        }

        return new Panel(table)
            .Header(
                $"[bold yellow]Bots[/]  {state} [bold]{speed}[/] " +
                $"[bold]{turns}/{maximumTurns}[/] {controls}")
            .Border(BoxBorder.Rounded)
            .Padding(1, 0);
    }
}
