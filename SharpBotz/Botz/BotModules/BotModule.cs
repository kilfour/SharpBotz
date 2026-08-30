namespace SharpBotz.Botz.BotModules;

public abstract class BotModule
{
    private bool installed;

    private protected BotModule(
        ModuleId id,
        int weight)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(weight);
        Id = id;
        Weight = weight;
    }

    public ModuleId Id { get; }

    public int Weight { get; }

    public bool IsInstalled => installed;

    public void Install() => installed = true;

    public ModuleInfo GetInfo(int totalWeight) => CreateInfo(totalWeight);

    private protected abstract ModuleInfo CreateInfo(int totalWeight);
}
