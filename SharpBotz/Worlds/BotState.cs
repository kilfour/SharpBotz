using SharpBotz.Botz;

namespace SharpBotz.Worlds;

public class BotState(Bot bot, Position position, Direction facing)
{
    public Bot Bot { get; } = bot;
    public Position Position { get; private set; } = position;
    public Direction Facing { get; private set; } = facing;

    public void Update(Position position, Direction facing)
    {
        Position = position;
        Facing = facing;
    }
}