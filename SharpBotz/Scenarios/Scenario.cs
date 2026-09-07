using QuickFuzzr;
using QuickFuzzr.UnderTheHood;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Worlds;

namespace SharpBotz.Scenarios;


public class Scenario
{
    private Scenario(string name, Arena.ArenaBuilder arenaBuilder, int maximumTurns, Func<GameWorld, bool> complete)
    {
        Name = name;
        ArenaBuilder = arenaBuilder;
        MaximumTurns = maximumTurns;
        Complete = complete;
    }

    public static ScenarioArena Named(string name) => new(name);

    public class ScenarioArena(string name)
    {
        public ScenarioMaximumTurns Arena(Arena.ArenaBuilder arenaBuilder)
            => new(name, arenaBuilder);
    }

    public class ScenarioMaximumTurns(string name, Arena.ArenaBuilder arenaBuilder)
    {
        public ScenarioCompletesWhen MaximumTurns(int maximumTurns)
        {
            return new(name, arenaBuilder, maximumTurns);
        }
    }

    public class ScenarioCompletesWhen(string name, Arena.ArenaBuilder arenaBuilder, int maximumTurns)
    {
        public Scenario CompletesWhen(Func<GameWorld, bool> complete)
            => new(name, arenaBuilder, maximumTurns, complete);
    }

    public string Name { get; init; } = string.Empty;
    public Arena.ArenaBuilder ArenaBuilder { get; }
    public int MaximumTurns { get; }
    public Func<GameWorld, bool> Complete { get; }

    public SpawnBuilder Spawn(Func<Bot> botFactory)
        => new(this, botFactory);

    public class SpawnBuilder(Scenario scenario, Func<Bot> botFactory)
    {
        public SpawnPosition At(int x, int y)
            => new(scenario, botFactory, _ => new(x, y));

        public Scenario AtRandom()
        {
            scenario.botPlacements.Add(
                new(botFactory,
                s => Fuzzr.OneOf(scenario.ArenaBuilder.GetAvailablePositions())(s).Value, s => Fuzzr.Enum<Direction>()(s).Value));
            return scenario;
        }
    }

    private int maximumBotWeight = 100;
    public Scenario MaximumBotWeight(int maximumBotWeight)
    {
        this.maximumBotWeight = maximumBotWeight;
        return this;
    }

    private int randomWalls;
    public Scenario RandomWalls(int numberOfWalls)
    {
        randomWalls = numberOfWalls;
        return this;
    }
    private Arena BuildArena(BotState[] botStates, State state)
    {
        var botPositions = botStates.Select(a => a.Position);
        var availablePositions = ArenaBuilder.GetAvailablePositions().Where(a => !botPositions.Contains(a)).ToList();
        for (int i = 0; i < randomWalls; i++)
        {
            var positionIndex = Fuzzr.Int(0, availablePositions.Count)(state).Value;
            var position = availablePositions[positionIndex];
            availablePositions.RemoveAt(positionIndex);
            var direction = Fuzzr.Enum<Direction>()(state).Value;
            ArenaBuilder.AddWallAt(position.X, position.Y);
        }
        return ArenaBuilder.Build();
    }

    private readonly List<BotPlacement> botPlacements = [];

    public class SpawnPosition(Scenario scenario, Func<Bot> botFactory, Func<State, Position> position)
    {
        public Scenario Facing(Direction facing)
        {
            scenario.botPlacements.Add(new(botFactory, position, _ => facing));
            return scenario;
        }
    }

    private record BotPlacement(
        Func<Bot> BotFactory,
        Func<State, Position> Position,
        Func<State, Direction> Facing);

    public GameWorld CreateWorld(int? seed = null)
    {
        var state = CreateState(seed);
        BotState[] botStates = [.. botPlacements.Select(a => CreateBotState(a, state))];
        return new(
            BuildArena(botStates, state),
            botStates,
            MaximumTurns,
            Complete,
            state);
    }

    private BotState CreateBotState(BotPlacement placement, State state)
    {
        var bot = placement.BotFactory();
        if (bot.ModuleRack.TotalWeight > maximumBotWeight)
        {
            throw new ArgumentException(
                $"A bot cannot weigh more than {maximumBotWeight}. " +
                $"{bot.Name}'s module rack weighs {bot.ModuleRack.TotalWeight}.",
                nameof(placement));
        }
        return new BotState(
            bot,
            placement.Position(state),
            placement.Facing(state));
    }

    private static State CreateState(int? seed) =>
        seed is null ? new() : new(seed.Value);

}
