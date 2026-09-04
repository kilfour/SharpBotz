using QuickFuzzr.UnderTheHood;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Worlds.EffectResolving;

namespace SharpBotz.Worlds;

public class GameWorld
{
    public Arena Arena { get; }
    public int MaximumTurns { get; }

    public IReadOnlyList<BotState> Bots { get; }

    public int Seed => fuzzrState.Seed;

    private readonly Func<GameWorld, bool> complete;

    public bool GoalReached => complete(this);

    private readonly State fuzzrState;
    private readonly BotState[] botStates;
    private BotObservation[] observations;

    public int Turn { get; private set; } = 1;
    public bool IsComplete => maximumTurnsReached || complete(this);

    private bool maximumTurnsReached;

    private static State CreateState(int? seed) =>
        seed is null ? new() : new(seed.Value);

    public GameWorld(
        Arena arena,
        BotState[] botStates,
        int maximumTurns,
        Func<GameWorld, bool> complete,
        int? seed = null)
        : this(arena, botStates, maximumTurns, complete, CreateState(seed)) { }

    public GameWorld(
        Arena arena,
        BotState[] botStates,
        int maximumTurns,
        Func<GameWorld, bool> complete,
        State fuzzrState)
    {
        ArgumentNullException.ThrowIfNull(arena);
        ArgumentNullException.ThrowIfNull(botStates);
        this.fuzzrState = fuzzrState;
        Arena = arena;
        MaximumTurns = maximumTurns;
        this.complete = complete;
        this.botStates = [.. botStates];
        Bots = Array.AsReadOnly(this.botStates);
        observations = ScannerEffectsResolver.Observe(Arena, this.botStates);
    }

    public void Update()
    {
        IncrementTurn();
        HandleEffects([.. GetBotStateEffects()]);
    }

    private IEnumerable<BotStateEffect> GetBotStateEffects() =>
        Bots.Select((botState, botIndex) =>
            new BotStateEffect(
                botState,
                botState.Bot.IsAlive
                    ? botState.Bot.GetEffects(observations[botIndex], fuzzrState)
                    : ModuleEffects.From([])));

    private void IncrementTurn()
    {
        if (Turn == MaximumTurns)
            maximumTurnsReached = true;
        else
            Turn++;
    }


    private void HandleEffects(BotStateEffect[] botEffects)
    {
        ReactorEffectsResolver.Handle(botEffects);
        RotatorEffectsResolver.Handle(botStates, botEffects);
        MovementEffectResolver.Handle(Arena, botStates, botEffects);
        MeleeEffectsResolver.Handle(botEffects);
        RangedEffectsResolver.Handle(Arena, botEffects);
        BatteryEffectsResolver.Handle(botEffects);
        observations = ScannerEffectsResolver.Handle(Arena, botEffects);
    }
}
