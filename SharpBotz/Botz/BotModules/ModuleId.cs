namespace SharpBotz.Botz.BotModules;

public readonly record struct ModuleId
{
    private string Value { get; }

    private ModuleId(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    public static ModuleId Is(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));
        return new ModuleId(id);
    }
}
