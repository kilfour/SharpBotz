using QuickPulse.Explains;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.C_PoweredModules;

[DocFile]
[DocContent(
"""
We already saw the modules related to energy generation and storage.  
All other modules consume power. The are all powered modules.

Let's explain the main mechanics by defining a (useless) one for demonstration purposes.
"""
)]
public class A_WhatIsAPoweredModule
{

}

public class DeadWeight : PoweredModule
{
    public DeadWeight(ModuleId id, int weight, int activationPower, int maximumPower)
        : base(id, weight, activationPower, maximumPower)
    {
    }

    protected override IEnumerable<ModuleEffect> CreateEffects()
    {
        throw new NotImplementedException();
    }

    protected override ModuleInfo CreateInfo(int totalWeight)
    {
        throw new NotImplementedException();
    }
}
