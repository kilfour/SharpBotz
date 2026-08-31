namespace SharpBotz.Botz.BotModules;

public class ModuleControl(IReadOnlyList<ModuleInfo> Modules)
{
    public TModule? FindModule<TModule>()
        where TModule : ModuleInfo =>
        Modules.OfType<TModule>().FirstOrDefault();

    public IReadOnlyList<TModule> FindModules<TModule>()
        where TModule : ModuleInfo =>
        [.. Modules.OfType<TModule>()];

    public TModule RequireModule<TModule>()
        where TModule : ModuleInfo =>
        FindModule<TModule>() ?? throw new InvalidOperationException(
            $"This bot does not have a {typeof(TModule).Name} module.");
}
