
using SharpBotz.Challenges.A_DeadAhead;
using SharpBotz.Spectre;

var scenario = DeadAhead.Challenge;
var world = scenario.CreateWorld();
var display = new SpectreGameDisplay();
await display.RunAsync(world, scenario.Name);

