using System.ComponentModel;
using QuickFuzzr.UnderTheHood;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Botz;

public class Bot
{
    public const int MaximumHitPoints = 100;

    private readonly BotBrain brain;

    protected Bot(string name, BotBrain brain, ModuleRack moduleRack)
    {

        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(brain);
        ArgumentNullException.ThrowIfNull(moduleRack);

        Name = name;
        this.brain = brain;
        ModuleRack = moduleRack;
        moduleRack.Attach();
    }

    public static BotNamed Named(string name) =>
        new(name);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BotNamed(string name)
    {
        public BotBrained Brain(BotBrain brain) =>
            new(name, brain);

        public class BotBrained(string name, BotBrain brain)
        {
            public Bot Rack(ModuleRack rack) =>
                new(name, brain, rack);
        }
    }

    public string Name { get; }
    public BotBrain Brain => brain;
    public ModuleRack ModuleRack { get; }

    public ModuleEffects GetEffects(BotObservation observation, State state) =>
        ModuleEffects.From(
            ModuleRack.Resolve(
                Brain.Decide(ModuleRack.GetModuleControl(), observation, state)));

    public int HitPoints { get; private set; } = MaximumHitPoints;

    public void TakeDamage(int damage) =>
        HitPoints = Math.Max(0, HitPoints - damage);

    public bool IsAlive => HitPoints > 0;
}
