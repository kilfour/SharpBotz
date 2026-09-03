
using SharpBotz.Challenges.A_DeadAhead;
using SharpBotz.Challenges.B_DifferentRoutes;
using SharpBotz.Spectre;

var scenario = DifferentRoutes.RouteOne;
var world = scenario.CreateWorld();
var display = new SpectreGameDisplay();
await display.RunAsync(world, scenario.Name);

