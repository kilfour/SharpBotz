using System.Numerics;
using QuickFuzzr.UnderTheHood;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Worlds;

public class GameWorld
{
    public Arena Arena { get; }
    public IReadOnlyList<BotState> Bots { get; }

    public int Seed => fuzzrState.Seed;
    private readonly State fuzzrState;
    private readonly BotState[] botStates;

    public int Turn { get; private set; }


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
        this.botStates = [.. botStates];
        Bots = Array.AsReadOnly(this.botStates);
    }

    public void Update()
    {
        Turn++;
        var botEffects = Bots.Select(a => (a, a.Bot.GetEffects(new BotObservation(), fuzzrState)));
        HandleEffects([.. botEffects]);
    }

    private static void HandleEffects((BotState BotState, ModuleEffects Effects)[] botEffects)
    {
        HandleReactorEffects(botEffects);
    }

    private static void HandleReactorEffects((BotState BotState, ModuleEffects Effects)[] botEffects)
    {
        foreach (var botEffect in botEffects)
        {
            HandleReactorEffect(botEffect);
        }
    }

    private static void HandleReactorEffect((BotState BotState, ModuleEffects Effects) botEffect)
    {
        var bot = botEffect.BotState.Bot;
        var reactorEffects = botEffect.Effects.ReactorEffects;
        foreach (var effect in reactorEffects)
        {
            switch (effect)
            {
                case ReactorOverLoadedEffect:
                    bot.TakeDamage(10);
                    break;

                default:
                    throw new ArgumentException("Unknown reactor effect supplied.");
            }
        }
    }
}
