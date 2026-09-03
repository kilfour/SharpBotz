namespace SharpBotz.Botz;

public class BotObservation
{
    public BotObservation() : this(new ScanResult[0, 0]) { }

    public BotObservation(ScanResult[,] scan) => Scan = new(scan);

    public BotScan Scan { get; }
}
