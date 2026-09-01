namespace SharpBotz.Botz;

public class BotState(Bot bot)
{
    public Bot Bot { get; } = bot;
    public Position Position { get; private set; }
    public Direction Facing { get; private set; }

    public void Update(Position position, Direction facing)
    {
        Position = position;
        Facing = facing;
    }
}