namespace SharpBotz.Botz.BotModules.Batteries;

public class Battery : BotModule
{
    private Battery(ModuleId id, int capacity)
        : base(id, GetWeight(capacity))
    {
        Capacity = capacity;
    }

    public static BatteryCapacity Named(string moduleId) =>
        new(ModuleId.Is(moduleId));

    public class BatteryCapacity(ModuleId id)
    {
        public Battery Capacity(int capacity) =>
            new(id, capacity);
    }

    public int Capacity { get; }

    public int Charge { get; private set; }

    public void Store(int amount, List<ModuleEffect> effects)
    {
        Charge += amount;
        if (Charge > Capacity)
        {
            Charge = 0;
            effects.Add(new BatteryOverChargedEffect(Id));
        }
    }

    public bool Drain(int amount, List<ModuleEffect> effects)
    {
        if (amount > Charge)
        {
            Charge = 0;
            effects.Add(new BatteryDrainedEffect(Id));
            return false;
        }
        Charge -= amount;
        return true;
    }

    public void Empty() => Charge = 0;

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new BatteryInfo(Id, Weight, Capacity, Charge);

    private static int GetWeight(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        var capacityWeight = (capacity / 25) + (capacity % 25 == 0 ? 0 : 1);
        return 2 + capacityWeight;
    }
}
