using SharpBotz.Botz;
using SharpBotz.Worlds;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace SharpBotz.Spectre;

internal static class WorldEventRenderer
{
    private const int MaximumVisibleEntries = 10;

    public static IRenderable Render(
        IReadOnlyList<(int Turn, WorldEvent Event)> eventLog,
        IReadOnlyList<BotState> bots,
        Bot? botFilter)
    {
        var relevantEntries = botFilter is null
            ? eventLog
            : eventLog.Where(entry => Concerns(entry.Event, botFilter));
        var entries = relevantEntries.TakeLast(MaximumVisibleEntries).ToArray();
        IRenderable content = entries.Length == 0
            ? RenderEmptyMessage(botFilter)
            : new Rows(entries.Select(entry => Render(entry, bots)));

        return new Panel(content)
            .Header(
                $"[bold yellow]Recent events[/] [grey]-[/] " +
                RenderFilter(botFilter, bots))
            .Border(BoxBorder.Rounded)
            .Padding(1, 0);
    }

    private static bool Concerns(WorldEvent worldEvent, Bot bot) =>
        worldEvent switch
        {
            BotDamaged damaged =>
                ReferenceEquals(damaged.Bot, bot) ||
                damaged.Cause is DamageCause.MeleeAttack melee &&
                    ReferenceEquals(melee.Attacker, bot) ||
                damaged.Cause is DamageCause.RangedAttack ranged &&
                    ReferenceEquals(ranged.Attacker, bot),
            _ => false,
        };

    private static IRenderable RenderEmptyMessage(Bot? botFilter) =>
        new Markup(botFilter is null
            ? "[grey]Events will appear after the first turn.[/]"
            : $"[grey]No events for {Markup.Escape(botFilter.Name)} yet.[/]");

    private static string RenderFilter(
        Bot? botFilter,
        IReadOnlyList<BotState> bots)
    {
        if (botFilter is null)
        {
            return "[white]All bots[/]";
        }

        var color = GetBotColor(botFilter, bots);
        return $"[{color}]{Markup.Escape(botFilter.Name)}[/]";
    }

    private static IRenderable Render(
        (int Turn, WorldEvent Event) entry,
        IReadOnlyList<BotState> bots) =>
        entry.Event switch
        {
            BotDamaged damaged => RenderDamage(entry.Turn, damaged, bots),
            _ => new Markup(
                $"[grey]T{entry.Turn,3}[/] " +
                $"[white]{Markup.Escape(entry.Event.ToString() ?? string.Empty)}[/]"),
        };

    private static IRenderable RenderDamage(
        int turn,
        BotDamaged damaged,
        IReadOnlyList<BotState> bots)
    {
        var subject = damaged.Cause switch
        {
            DamageCause.MeleeAttack melee => melee.Attacker,
            DamageCause.RangedAttack ranged => ranged.Attacker,
            _ => damaged.Bot,
        };
        var messageColor = damaged.Cause is DamageCause.MeleeAttack or
            DamageCause.RangedAttack or
            DamageCause.Collision
                ? "yellow"
                : "red";

        return new Markup(
            $"[grey]T{turn,3}[/] " +
            $"[{GetBotColor(subject, bots)}]{Markup.Escape(subject.Name)}[/] " +
            $"[{messageColor}]{Markup.Escape(Describe(damaged))}[/]");
    }

    private static string Describe(BotDamaged damaged) =>
        damaged.Cause switch
        {
            DamageCause.Collision =>
                $"takes {damaged.Damage} collision damage",
            DamageCause.ReactorOverload cause =>
                $"overloads {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.BatteryDrained cause =>
                $"overdraws {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.BatteryOvercharged cause =>
                $"overcharges {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.PowerNotStored cause =>
                $"cannot store power from {cause.ModuleId} and takes {damaged.Damage} damage",
            DamageCause.ThrusterOvercharged cause =>
                $"overcharges {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.RotatorOvercharged cause =>
                $"overcharges {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.MeleeAttack cause =>
                $"hits {damaged.Bot.Name} with {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.MeleeOvercharged cause =>
                $"overcharges {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.RangedAttack cause =>
                $"hits {damaged.Bot.Name} with {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.RangedOvercharged cause =>
                $"overcharges {cause.ModuleId} for {damaged.Damage} damage",
            DamageCause.ScannerOvercharged cause =>
                $"overcharges {cause.ModuleId} for {damaged.Damage} damage",
            _ => $"takes {damaged.Damage} damage",
        };

    private static string GetBotColor(Bot bot, IReadOnlyList<BotState> bots)
    {
        for (var index = 0; index < bots.Count; index++)
        {
            if (ReferenceEquals(bots[index].Bot, bot))
            {
                return BotPalette.GetColors(index).Foreground;
            }
        }

        return "white";
    }
}
