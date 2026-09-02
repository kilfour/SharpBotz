namespace SharpBotz.Botz;

public abstract record ScanResult
{
    private ScanResult() { }

    public sealed record OutOfBounds : ScanResult;

    public sealed record Empty : ScanResult;

    public sealed record Wall : ScanResult;

    public sealed record OwnBot(Direction Facing, int HitPoints) : ScanResult;

    public sealed record Bot(Direction Facing, int HitPoints) : ScanResult;
}
