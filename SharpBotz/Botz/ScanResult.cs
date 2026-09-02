namespace SharpBotz.Botz;

public abstract record ScanResult
{
    private ScanResult() { }

    public record OutOfBounds : ScanResult;

    public record Empty : ScanResult;

    public record Wall : ScanResult;

    public record OwnBot(Direction Facing, int HitPoints) : ScanResult;

    public record Bot(Direction Facing, int HitPoints) : ScanResult;
}
