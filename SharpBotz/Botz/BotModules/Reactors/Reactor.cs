namespace SharpBotz.Botz.BotModules.Reactors;


public class Reactor : BotModule
{
    // ------------------------------------------------------------------------------
    // -- Construction
    private Reactor(ModuleId id, int maximumOutput)
        : base(id, GetWeight(maximumOutput)) => MaximumOutput = maximumOutput;

    public static ReactorMaximumOutput Named(string moduleId) =>
        new(ModuleId.Is(moduleId));

    public class ReactorMaximumOutput(ModuleId id)
    {
        public Reactor MaximumOutput(int maximumOutput) =>
            new(id, maximumOutput);
    }
    // ------------------------------------------------------------------------------


    public int MaximumOutput { get; }

    public PowerGeneration SetOutput(int output)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(output);
        return new(Id, output);
    }

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new ReactorInfo(Id, MaximumOutput);

    private static int GetWeight(int outputPerTurn)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(outputPerTurn);
        var outputSquared = (long)outputPerTurn * outputPerTurn;
        var outputWeight = (outputSquared / 25) +
                           (outputSquared % 25 == 0 ? 0 : 1);
        return 2 + (int)outputWeight;
    }
}
