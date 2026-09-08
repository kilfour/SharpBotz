# SharpBotz Challenges

This project contains a sequence of programming challenges for learning SharpBotz. Each challenge asks you to equip a bot and program its brain so that it completes every scenario for that challenge.

`A_DeadAhead` is a completed example. Read [`AheadBot.cs`](A_DeadAhead/AheadBot.cs) to see how a bot installs modules, inspects its observation, and creates a power plan to scan, move, or attack. The accompanying [`DeadAhead.cs`](A_DeadAhead/DeadAhead.cs) shows how a challenge defines the arena, starting positions, turn limit, and winning condition. You can run this scenario in the challenge console before starting the exercises.

Begin your own implementation with [`B_DifferentRoutes/RouteBot.cs`](B_DifferentRoutes/RouteBot.cs), then continue through the remaining lettered directories in order.

## Working on a challenge

For each challenge from `B_DifferentRoutes` onward:

1. Open its `*Bot.cs` file.
2. Choose the modules to install in its `ModuleRack`.
3. Implement the brain's `RoutePower` method.
4. Run the scenario in the [challenge console](../SharpBotz.Challenges.Console/README.md).
5. Enable the corresponding skipped test in [`ChallengeTests.cs`](../SharpBotz.Challenges.Tests/ChallengeTests.cs) and make it pass.

The bot you control is always spawned first. A scenario succeeds when its completion condition is met before the maximum number of turns. Some later challenges have multiple scenarios; one implementation must handle all scenarios belonging to that challenge.

Work through the directories in order, from `B_DifferentRoutes` through `J_TrialByFire`. Each challenge builds on concepts demonstrated by `A_DeadAhead` and introduced by the earlier exercises.

The main [SharpBotz documentation](../Docs/ToC.md) explains arenas, modules, bot brains, observations, and turns.

## Useful commands

From the repository root:

```console
dotnet build SharpBotz.sln
dotnet test SharpBotz.Challenges.Tests
dotnet run --project SharpBotz.Challenges.Console
```

The `DeadAhead_Challenge` test is enabled because it covers the completed example. The exercise tests begin skipped so that unfinished challenges do not make the entire solution fail. Remove `Skip = "Not Implemented"` from a challenge test when you begin working on it.
