namespace SharpBotz.Botz;


public class Bot
{
    public const int MaximumHitPoints = 100;
    public const int MaximumWeight = 100;

    // private readonly BotBrain brain;
    // private IReadOnlyList<ModuleEffect> activeEffects = [];

    // public Bot(BotBrain brain)
    //     : this(brain, BotModuleRack.Standard()) { }

    // public Bot(BotBrain brain, BotModuleRack moduleRack)
    // {
    //     ArgumentNullException.ThrowIfNull(brain);
    //     ArgumentNullException.ThrowIfNull(moduleRack);

    //     if (moduleRack.TotalWeight > MaximumWeight)
    //     {
    //         throw new ArgumentException(
    //             $"A bot cannot weigh more than {MaximumWeight}; " +
    //             $"this module rack weighs {moduleRack.TotalWeight}.",
    //             nameof(moduleRack));
    //     }

    //     moduleRack.Attach();
    //     this.brain = brain;
    //     ModuleRack = moduleRack;
    // }

    // public string Name => brain.Name;

    // public BotBrain Brain => brain;

    // public BotModuleRack ModuleRack { get; }

    public int HitPoints { get; private set; } = MaximumHitPoints;

    public void TakeDamage(int damage) =>
        HitPoints = Math.Max(0, HitPoints - damage);

    // public void TakeDamage(int damage, Bot attacker)
    // {
    //     var wasAlive = IsAlive;
    //     var previousHitPoints = HitPoints;
    //     TakeDamage(damage);

    //     if (!ReferenceEquals(this, attacker))
    //     {
    //         attacker.DamageDealt = checked(
    //             attacker.DamageDealt + previousHitPoints - HitPoints);
    //         if (wasAlive && !IsAlive)
    //         {
    //             attacker.Kills++;
    //         }
    //     }
    // }

    public bool IsAlive => HitPoints > 0;

    public Direction Facing { get; private set; } = Direction.Up;

    // private Position position = new(1, 1);

    // public Position Position => position;

    public bool HasSpawned { get; private set; }

    // public void Spawn(Position spawnPosition, Direction spawnDirection)
    // {
    //     position = spawnPosition;
    //     facing = spawnDirection;
    //     HasSpawned = true;
    // }

    // public int Cooldown { get; public set; }

    // public int BatteryLevel => ModuleRack.BatteryLevel;

    // public int BatteryCapacity => ModuleRack.BatteryCapacity;

    // public int ReactorOutput => ModuleRack.ReactorOutput;

    // public int MaximumReactorOutput => ModuleRack.MaximumReactorOutput;

    // public int Weight => ModuleRack.TotalWeight;

    // public int ScannerRange =>
    //     activeEffects
    //         .OfType<ScanEffect>()
    //         .Select(effect => effect.Range)
    //         .DefaultIfEmpty()
    //         .Max();

    // public int ScannerViewSize => checked((ScannerRange * 2) + 1);

    // public void ApplyState(Position nextPosition, Direction nextDirection)
    // {
    //     DistanceTravelled = checked(
    //         DistanceTravelled +
    //         Math.Abs(nextPosition.X - position.X) +
    //         Math.Abs(nextPosition.Y - position.Y));
    //     position = nextPosition;
    //     facing = nextDirection;
    // }

    // public BotObservation Observe(ScanResult[,] scan) =>
    //     new(
    //         scan,
    //         Facing,
    //         BatteryLevel,
    //         BatteryCapacity,
    //         ReactorOutput,
    //         ModuleRack.TotalWeight,
    //         ModuleRack.Modules);

    // public bool TryGeneratePower() => ModuleRack.TryGeneratePower();

    // public PowerPlan Decide(BotObservation observation, State fuzzrState) =>
    //     brain.Decide(observation, fuzzrState);

    // public IReadOnlyList<ModuleEffect> RoutePower(
    //     PowerPlan plan,
    //     Action<GameLogEventType, string>? writeLog = null)
    // {
    //     if (!ModuleRack.TryValidate(plan, out var validated))
    //     {
    //         Malfunction(3, 0);
    //         writeLog?.Invoke(
    //             GameLogEventType.Malfunction,
    //             "invalid power plan; modules offline for 3 turns");
    //         return [];
    //     }

    //     ModuleRack.SetReactorOutputs(validated);
    //     var availablePower = BatteryLevel;
    //     if (!ModuleRack.TryConsumePower(validated.TotalPower))
    //     {
    //         Malfunction(3, 0);
    //         writeLog?.Invoke(
    //             GameLogEventType.Malfunction,
    //             $"requested {validated.TotalPower} power with only {availablePower} available; " +
    //             "batteries emptied and modules offline for 3 turns");
    //         return [];
    //     }

    //     activeEffects = ModuleRack.Apply(validated);
    //     return activeEffects;
    // }

    // public void Malfunction(int turns, int damage)
    // {
    //     Cooldown = turns;
    //     TakeDamage(damage);
    //     DisconnectModules();
    // }

    // public void DisconnectModules()
    // {
    //     ModuleRack.Disconnect();
    //     activeEffects = [];
    // }
}
