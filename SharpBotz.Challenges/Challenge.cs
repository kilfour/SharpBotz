using SharpBotz.Botz;
using SharpBotz.Worlds;

namespace SharpBotz.Challenges;

public abstract class Challenge
{
    protected static bool OnlyFirstBotLives(GameWorld world)
        => world.Bots[0].Bot.IsAlive &&
           world.Bots.Skip(1).All(bot => !bot.Bot.IsAlive);

    protected static bool OnlyFirstBotLivesAt(
        GameWorld world,
        Position position)
        => OnlyFirstBotLives(world) &&
           world.Bots[0].Position.Equals(position);

    protected static bool OnlyFirstBotLivesWithStoredPower(
        GameWorld world,
        int minimumStoredPower)
        => OnlyFirstBotLives(world) &&
           world.Bots[0].Bot.ModuleRack.BatteryLevel >= minimumStoredPower;
}
