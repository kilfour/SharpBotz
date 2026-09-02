namespace SharpBotz.Botz.BotModules.Scanners;


public sealed class Scanner(ModuleId id, int powerPerRange, int maximumPower)
    : PoweredModule(id, GetWeight(powerPerRange))
{
    private const int StandardPowerPerRange = 3;

}
