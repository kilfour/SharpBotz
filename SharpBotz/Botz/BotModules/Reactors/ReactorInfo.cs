namespace SharpBotz.Botz.BotModules.Reactors;


public record ReactorInfo(
    ModuleId Id,
    int MaximumOutput)
    : ModuleInfo(Id)
{
    public PowerGeneration SetOutput(int output)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(output);
        return new(Id, output);
    }
}
