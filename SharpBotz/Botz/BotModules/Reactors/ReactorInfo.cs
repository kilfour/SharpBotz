namespace SharpBotz.Botz.BotModules.Reactors;


public record ReactorInfo(
    ModuleId Id,
    int Weight,
    int MaximumOutput,
    int CurrentOutput)
    : ModuleInfo(Id)
{
    public int OutputPerTurn => MaximumOutput;

    // public PowerAllocation SetOutput(int output)
    // {
    //     ArgumentOutOfRangeException.ThrowIfNegative(output);
    //     ArgumentOutOfRangeException.ThrowIfGreaterThan(output, MaximumOutput);
    //     return new(Id, output);
    // }
}
