namespace SharpBotz.Botz.BotModules.Batteries;

public class Battery : BotModule
{
    private Battery(ModuleId id, int capacity)
        : base(id, GetWeight(capacity))
    {
        Capacity = capacity;
    }

    public static Battery Create(ModuleId id, int capacity)
        => new(id, capacity);

    public int Capacity { get; }

    public int Charge { get; private set; }

    public int AvailableCapacity => Capacity - Charge;

    public void Store(int amount) => Charge += amount;

    public int Drain(int amount)
    {
        var drained = Math.Min(amount, Charge);
        Charge -= drained;
        return drained;
    }

    public void Empty() => Charge = 0;

    private protected override ModuleInfo CreateInfo(int totalWeight) =>
        new BatteryInfo(Id, Weight, Capacity, Charge);

    private static int GetWeight(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        var capacityWeight = (capacity / 25) + (capacity % 25 == 0 ? 0 : 1);
        return checked(2 + capacityWeight);
    }
}
