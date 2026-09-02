using System.Reflection;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.RangedWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Rotators;
using SharpBotz.Botz.BotModules.Scanners;

namespace SharpBotz.Tests;

public class ModuleConstructionPatternTests
{
    [Theory]
    [InlineData(typeof(Battery))]
    [InlineData(typeof(Drive))]
    [InlineData(typeof(Melee))]
    [InlineData(typeof(Ranged))]
    [InlineData(typeof(Reactor))]
    [InlineData(typeof(Rotator))]
    [InlineData(typeof(Scanner))]
    public void ModulesUseNamedStagedConstruction(Type moduleType)
    {
        Assert.Empty(moduleType.GetConstructors());

        var named = Assert.Single(moduleType
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => method.Name == "Named"));
        var moduleId = Assert.Single(named.GetParameters());
        Assert.Equal(typeof(string), moduleId.ParameterType);
    }
}
