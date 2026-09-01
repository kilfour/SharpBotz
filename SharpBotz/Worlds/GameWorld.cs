using QuickFuzzr;
using QuickFuzzr.UnderTheHood;
using SharpBotz.Arenas;
using SharpBotz.Botz;

namespace SharpBots.Engine;

public class GameWorld
{
    public int Seed => fuzzrState.Seed;

    public Arena Arena { get; }

    public IReadOnlyList<Bot> Bots { get; }

    public int Turn { get; private set; }

    public bool IsGameOver => bots.Count(bot => bot.IsAlive) <= 1;

    public Bot? Winner => IsGameOver
        ? bots.SingleOrDefault(bot => bot.IsAlive)
        : null;

    private readonly Bot[] bots;
    private readonly State fuzzrState;

    // private readonly ModuleSystem modules;
    // private readonly Intentions intentions;
    // private readonly Collisions collisions;

    private static State CreateState(int? seed) =>
        seed is null ? new() : new(seed.Value);

    public GameWorld(
        Arena arena,
        int? seed = null)
        : this(arena, CreateState(seed)) { }

    private GameWorld(
        Arena arena,
        State fuzzrState)
    {
        ArgumentNullException.ThrowIfNull(arena);
        ArgumentNullException.ThrowIfNull(bots);
        this.fuzzrState = fuzzrState;
        Arena = arena;
        Bots = Array.AsReadOnly(this.bots);

        // modules = new ModuleSystem(this.bots);
        // intentions = new Intentions(Arena, this.bots, fuzzrState, captureLog);
        // collisions = new Collisions(Arena);
    }

    public void Update()
    {
        Turn++;
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
