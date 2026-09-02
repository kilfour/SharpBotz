using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Worlds;

namespace SharpBotz.Scenarios;


public class Scenario
{
    private Scenario(string name, Arena arena, int maximumTurns, Func<GameWorld, bool> complete)
    {
        Name = name;
        Arena = arena;
        MaximumTurns = maximumTurns;
        Complete = complete;
    }

    public static ScenarioArena Named(string name) => new(name);

    public class ScenarioArena(string name)
    {
        public ScenarioMaximumTurns Arena(Arena arena)
            => new(name, arena);
    }

    public class ScenarioMaximumTurns(string name, Arena arena)
    {
        public ScenarioCompletesWhen MaximumTurns(int maximumTurns)
        {
            return new(name, arena, maximumTurns);
        }

    }

    public class ScenarioCompletesWhen(string name, Arena arena, int maximumTurns)
    {
        public Scenario CompletesWhen(Func<GameWorld, bool> complete)
            => new(name, arena, maximumTurns, complete);
    }

    public string Name { get; init; } = string.Empty;
    public Arena Arena { get; }
    public int MaximumTurns { get; }
    public Func<GameWorld, bool> Complete { get; }

    public SpawnBuilder Spawn(Func<Bot> botFactory)
        => new(this, botFactory);

    public class SpawnBuilder(Scenario scenario, Func<Bot> botFactory)
    {
        public SpawnPosition At(int x, int y)
            => new(scenario, botFactory, new(x, y));
    }

    private readonly List<BotPlacement> botPlacements = [];

    public class SpawnPosition(Scenario scenario, Func<Bot> botFactory, Position position)
    {
        public Scenario Facing(Direction facing)
        {
            scenario.botPlacements.Add(new(botFactory, position, facing));
            return scenario;
        }
    }

    private record BotPlacement(Func<Bot> BotFactory, Position Position, Direction Facing);

    public GameWorld CreateWorld(int? seed = null) =>
        new(
            Arena,
            [.. botPlacements.Select(placement =>
                    new BotState(
                        placement.BotFactory(),
                        placement.Position,
                        placement.Facing))],
            MaximumTurns,
            Complete,
            seed);
}
