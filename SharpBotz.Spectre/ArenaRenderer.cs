using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Worlds;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Text;

namespace SharpBotz.Spectre;

internal static class ArenaRenderer
{
    public static IRenderable Render(Arena arena, IReadOnlyList<BotState> bots)
    {
        var rows = new List<IRenderable>(arena.Height);
        for (var y = 0; y < arena.Height; y++)
        {
            var row = new StringBuilder(arena.Width * 2);
            for (var x = 0; x < arena.Width; x++)
            {
                row.Append(RenderTile(arena[x, y], FindBotAt(bots, x, y)));
            }

            rows.Add(new Markup(row.ToString()));
        }

        return new Rows(rows);
    }

    private static (int Index, Direction Facing)? FindBotAt(
        IReadOnlyList<BotState> bots,
        int x,
        int y)
    {
        for (var index = 0; index < bots.Count; index++)
        {
            var bot = bots[index];
            if (bot.Bot.IsAlive && bot.Position.X == x && bot.Position.Y == y)
            {
                return (index, bot.Facing);
            }
        }

        return null;
    }

    private static string RenderTile(
        ArenaTileType tileType,
        (int Index, Direction Facing)? bot)
    {
        if (bot is { } occupyingBot)
        {
            var (foreground, background) = BotPalette.GetColors(occupyingBot.Index);
            var glyph = occupyingBot.Facing switch
            {
                Direction.Up => "↑↑",
                Direction.Down => "↓↓",
                Direction.Left => "←←",
                Direction.Right => "→→",
                _ => "??",
            };
            return $"[bold {foreground} on {background}]{glyph}[/]";
        }

        return tileType switch
        {
            ArenaTileType.OutOfBounds => "[red]××[/]",
            ArenaTileType.Wall => "[grey]██[/]",
            ArenaTileType.Empty => "  ",
            _ => "[red]??[/]",
        };
    }
}
