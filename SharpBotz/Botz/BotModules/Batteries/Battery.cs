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

    public ModuleEffect? Store(int amount)
    {
        Charge += amount;
        if (Charge > Capacity)
        {
            var overCharge = Charge + amount - Capacity;
            Charge = 0;
            return new BatteryOverChargedEffect(Id, overCharge);
        }
        return null;
    }

    public ModuleEffect? Drain(int amount)
    {
        if (amount > Charge)
        {
            Charge = 0;
            return new BatteryDrainedEffect(Id);
        }
        Charge -= amount;
        return null;
    }

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new BatteryInfo(Id, Weight, Capacity, Charge);

    private static int GetWeight(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        var capacityWeight = (capacity / 25) + (capacity % 25 == 0 ? 0 : 1);
        return 2 + capacityWeight;
    }
}
