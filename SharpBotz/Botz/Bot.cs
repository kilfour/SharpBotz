using QuickFuzzr.UnderTheHood;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Botz;

public class Bot
{
    public const int MaximumHitPoints = 100;

    // move this to Scenario
    public const int MaximumWeight = 100;

    private readonly BotBrain brain;

    public Bot(BotBrain brain, ModuleRack moduleRack)
    {
        ArgumentNullException.ThrowIfNull(brain);
        ArgumentNullException.ThrowIfNull(moduleRack);

        // move this to Scenario
        if (moduleRack.TotalWeight > MaximumWeight)
        {
            throw new ArgumentException(
                $"A bot cannot weigh more than {MaximumWeight}; " +
                $"this module rack weighs {moduleRack.TotalWeight}.",
                nameof(moduleRack));
        }

        moduleRack.Attach();
        this.brain = brain;
        ModuleRack = moduleRack;
    }

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
