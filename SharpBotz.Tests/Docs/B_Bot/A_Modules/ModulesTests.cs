using QuickPulse.Explains;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules;

[DocFile]
public class ModulesTests
{
    [Fact]
    [DocContent("Every module is defined by a `ModuleId`.")]
    [DocExample(typeof(ModulesTests), nameof(ValidModelId))]
    public void ModuleIdAsString()
    {
        Assert.Equal("my-module", ValidModelId.ToString());
    }

    [CodeSnippet]
    public ModuleId ValidModelId =
        ModuleId.Is("my-module");

    [Fact]
    [DocContent("A `ModuleId` cannot be `null`, `string.Empty` or consist only of whitespace.")]
    public void InvalidModuleIds()
    {
        var nullEx = Assert.Throws<ArgumentNullException>(() => ModuleId.Is(null!));
        Assert.Equal("Value cannot be null. (Parameter 'id')", nullEx.Message);
        var emptyEx = Assert.Throws<ArgumentException>(() => ModuleId.Is(string.Empty));
        Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'id')", emptyEx.Message);
        var whitespaceEx = Assert.Throws<ArgumentException>(() => ModuleId.Is(" "));
        Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'id')", whitespaceEx.Message);
    }
}
