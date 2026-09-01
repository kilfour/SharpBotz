using SharpBotz.Arenas;
using SharpBotz.Botz;

namespace SharpBotz.Scenarios;


public class Scenario
{
    private Scenario(string name, ArenaWidth width, ArenaHeight height)
    {
        Name = name;
        this.width = width;
        this.height = height;
    }

    public static ScenarioNamed Named(string name) => new(name);

    public class ScenarioNamed(string name)
    {
        public Scenario ArenaSize(ArenaWidth width, ArenaHeight height)
            => new(name, width, height);
    }

    public string Name { get; init; } = string.Empty;
    private ArenaWidth width;
    private ArenaHeight height;
}
