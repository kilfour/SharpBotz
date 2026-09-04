# <img src="icon.png" width="40" align="top" alt="SharpBotz icon"/> SharpBotz

> **The Machine Rages Back**

[![Docs](https://img.shields.io/badge/docs-SharpBotz-blue?style=flat-square&logo=readthedocs)](Docs/ToC.md)
[![License: MIT](https://img.shields.io/badge/license-MIT-success?style=flat-square)](LICENSE)

SharpBotz is a programmable robot-combat sandbox for learning and experimenting with C#. Build a bot from weighted modules, write a brain that routes power to those modules each turn, and test the result in a collection of arena scenarios.

The engine models reactors, batteries, movement, rotation, melee and ranged weapons, scanners, collisions, and damage. Scenarios combine those systems into small problems that can be solved incrementally.

## Start here

You will need the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

Clone the repository, then build it from the repository root:

```console
dotnet build SharpBotz.sln
```

If you want to learn by solving increasingly difficult exercises, follow the [SharpBotz Challenges guide](SharpBotz.Challenges/README.md). To choose a scenario and watch it run:

```console
dotnet run --project SharpBotz.Challenges.Console
```

For an explanation of the API and its concepts, start with the [documentation contents](Docs/ToC.md). It covers arenas, bots, modules, brains, scenarios, and game-world turns.

## Repository guide

| Path | Purpose |
| --- | --- |
| [`SharpBotz`](SharpBotz) | Core simulation and public API |
| [`Docs`](Docs/ToC.md) | Generated learning and API documentation |
| [`SharpBotz.Challenges`](SharpBotz.Challenges/README.md) | Bots and scenarios for the learner exercises |
| [`SharpBotz.Challenges.Console`](SharpBotz.Challenges.Console/README.md) | Interactive challenge visualizer |
| [`SharpBotz.Challenges.Tests`](SharpBotz.Challenges.Tests) | Executable success criteria for the challenges |
| [`SharpBotz.Spectre`](SharpBotz.Spectre) | Terminal renderer used by the console applications |
| [`SharpBotz.Tests`](SharpBotz.Tests) | Engine tests and source material for the generated docs |

## Tests

Run the complete test suite with:

```console
dotnet test SharpBotz.sln
```

Individual challenge tests are intentionally skipped until their corresponding learner solution is ready. The engine test suite and scenario smoke test still run normally.

SharpBotz is available under the [MIT License](LICENSE).
