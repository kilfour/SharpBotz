using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Worlds;

namespace SharpBotz.Scenarios;


public class Scenario
{
    private Scenario(string name, Arena arena)
    {
        Name = name;
        Arena = arena;
    }

    public static ScenarioNamed Named(string name) => new(name);

    public class ScenarioNamed(string name)
    {
        public Scenario Arena(Arena arena)
            => new(name, arena);
    }

    public string Name { get; init; } = string.Empty;
    public Arena Arena { get; }

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

    public GameWorld Start() =>
        new(
            Arena,
            [.. botPlacements.Select(placement =>
                    new BotState(
                        placement.BotFactory(),
                        placement.Position,
                        placement.Facing))]);
}
