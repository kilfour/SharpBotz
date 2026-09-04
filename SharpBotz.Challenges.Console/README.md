# SharpBotz Challenge Console

This console application lets you watch any challenge scenario play out. It is intended as a fast feedback tool while developing the bots in [`SharpBotz.Challenges`](../SharpBotz.Challenges/README.md).

## Run it

From the repository root:

```console
dotnet run --project SharpBotz.Challenges.Console
```

Choose a scenario from the menu. The arena display shows each bot's position and direction, while the table shows its name, hit points, battery level, reactor output, weight, and status.

## Controls

- `Left Arrow` or `-`: run more slowly
- `Right Arrow` or `+`: run more quickly
- `Space`: pause or resume
- `Enter`: advance one turn while paused
- `Ctrl+C`: stop the application

After changing a bot or brain, stop and rerun the console to rebuild the project and start a fresh scenario.

The visualizer helps explain what happened, but the challenge tests determine whether a solution satisfies the winning condition. See the [challenge README](../SharpBotz.Challenges/README.md) for the suggested workflow.
