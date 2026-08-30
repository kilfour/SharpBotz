namespace SharpBotz.Botz.BotModules.Reactors;


public class Reactor : BotModule
{
    private Reactor(ModuleId id, int outputPerTurn)
        : base(id, GetWeight(outputPerTurn))
    {
        MaximumOutput = outputPerTurn;
        CurrentOutput = outputPerTurn;
    }

    public static Reactor Create(string moduleId, int capacity)
        => new(ModuleId.Is(moduleId), capacity);

    public int MaximumOutput { get; }

    public int OutputPerTurn => MaximumOutput;

    public int CurrentOutput { get; private set; }

    public void SetOutput(int output) => CurrentOutput = output;

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new ReactorInfo(Id, Weight, MaximumOutput, CurrentOutput);

    private static int GetWeight(int outputPerTurn)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(outputPerTurn);
        var outputSquared = (long)outputPerTurn * outputPerTurn;
        var outputWeight = (outputSquared / 25) +
                           (outputSquared % 25 == 0 ? 0 : 1);
        return 2 + (int)outputWeight;
    }
}
