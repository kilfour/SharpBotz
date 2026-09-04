# SharpBotz Challenges

This project contains a sequence of programming challenges for learning SharpBotz. Your job is to equip each challenge bot and program its brain so that it completes every scenario for that challenge.

Start with [`A_DeadAhead/AheadBot.cs`](A_DeadAhead/AheadBot.cs). The accompanying [`DeadAhead.cs`](A_DeadAhead/DeadAhead.cs) defines the arena, starting positions, turn limit, and winning condition.

## Working on a challenge

For each lettered challenge:

1. Open its `*Bot.cs` file.
2. Choose the modules to install in its `ModuleRack`.
3. Implement the brain's `RoutePower` method.
4. Run the scenario in the [challenge console](../SharpBotz.Challenges.Console/README.md).
5. Enable the corresponding skipped test in [`ChallengeTests.cs`](../SharpBotz.Challenges.Tests/ChallengeTests.cs) and make it pass.

The bot you control is always spawned first. A scenario succeeds when its completion condition is met before the maximum number of turns. Some later challenges have multiple scenarios; one implementation must handle all scenarios belonging to that challenge.

Work through the directories in order, from `A_DeadAhead` through `J_TrialByFire`. Each challenge builds on concepts introduced by the earlier ones.

The main [SharpBotz documentation](../Docs/ToC.md) explains arenas, modules, bot brains, observations, and turns.

## Useful commands

From the repository root:

```console
dotnet build SharpBotz.sln
dotnet test SharpBotz.Challenges.Tests
dotnet run --project SharpBotz.Challenges.Console
```

The challenge tests begin skipped so that unfinished exercises do not make the entire solution fail. Remove `Skip = "Not Implemented"` from a challenge test when you begin working on it.
