namespace SharpBotz.Botz;

public class BotObservation(ScanResult[,] scan)
{
    public BotObservation() : this(new ScanResult[0, 0]) { }

    public ScanResult[,] Scan { get; } = scan ??
        throw new ArgumentNullException(nameof(scan));
}
