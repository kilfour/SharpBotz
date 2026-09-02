using QuickFuzzr.UnderTheHood;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Worlds.EffectResolving;

namespace SharpBotz.Worlds;

public class GameWorld
{

    public Arena Arena { get; }
    public IReadOnlyList<BotState> Bots { get; }

    public int Seed => fuzzrState.Seed;
    private readonly State fuzzrState;
    private readonly BotState[] botStates;
    private BotObservation[] observations;

    public int Turn { get; private set; } = 1;

    private static State CreateState(int? seed) =>
        seed is null ? new() : new(seed.Value);

    public GameWorld(
        Arena arena,
        BotState[] botStates,
        int? seed = null)
        : this(arena, botStates, CreateState(seed)) { }

    public GameWorld(
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
        observations = ScannerEffectsResolver.Observe(Arena, this.botStates);
    }

    public void Update()
    {
        Turn++;
        var botEffects = Bots.Select((botState, botIndex) => new BotStateEffect(
            botState,
            botState.Bot.GetEffects(observations[botIndex], fuzzrState)));
        HandleEffects([.. botEffects]);
    }

    private void HandleEffects(BotStateEffect[] botEffects)
    {
        ReactorEffectsResolver.Handle(botEffects);
        RotatorEffectsResolver.Handle(botStates, botEffects);
        MovementEffectResolver.Handle(Arena, botStates, botEffects);
        MeleeEffectsResolver.Handle(botEffects);
        RangedEffectsResolver.Handle(Arena, botEffects);
        observations = ScannerEffectsResolver.Handle(Arena, botEffects);
    }
}
