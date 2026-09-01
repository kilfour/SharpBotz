using System.Numerics;
using QuickFuzzr.UnderTheHood;
using SharpBotz.Arenas;
using SharpBotz.Botz;

namespace SharpBotz.Worlds;

public class GameWorld
{
    public Arena Arena { get; }
    public IReadOnlyList<BotState> Bots { get; }

    public int Seed => fuzzrState.Seed;
    private readonly State fuzzrState;
    private readonly BotState[] botStates;

    public int Turn { get; private set; }

    // public bool IsGameOver => bots.Count(bot => bot.IsAlive) <= 1;

    // public Bot? Winner => IsGameOver
    //     ? bots.SingleOrDefault(bot => bot.IsAlive)
    //     : null;

    // private readonly ModuleSystem modules;
    // private readonly Intentions intentions;
    // private readonly Collisions collisions;

    private static State CreateState(int? seed) =>
        seed is null ? new() : new(seed.Value);

    public GameWorld(
        Arena arena,
        BotState[] botStates,
        int? seed = null)
        : this(arena, botStates, CreateState(seed)) { }

    private GameWorld(
        Arena arena,
        BotState[] botStates,
        State fuzzrState)
    {
        ArgumentNullException.ThrowIfNull(arena);
        ArgumentNullException.ThrowIfNull(botStates);
        this.fuzzrState = fuzzrState;
        Arena = arena;
        this.botStates = botStates;
        Bots = Array.AsReadOnly(this.botStates);

        // modules = new ModuleSystem(this.bots);
        // intentions = new Intentions(Arena, this.bots, fuzzrState, captureLog);
        // collisions = new Collisions(Arena);
    }

    public void Update()
    {
        Turn++;
        var botEffects = Bots.Select(a => (BotState: a, Plan: a.Bot.GetEffects(new BotObservation(), fuzzrState)));
        // var coolingDownAtStart = bots.Where(bot => bot.Cooldown > 0).ToArray();
        // modules.Handle(writeLog);
        // Arena.RedrawBots(bots);
        // var intents = intentions.Handle(writeLog);
        // collisions.Handle(intents, writeLog);
        // MeleeAttacks.Handle(intents, writeLog);
        // RangedAttacks.Handle(Arena, intents, writeLog);
        // Cooldowns.Handle(coolingDownAtStart);
        // Arena.RedrawBots(bots);
    }
}
