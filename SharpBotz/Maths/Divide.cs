namespace SharpBotz.Maths;

public class Divide
{
    public static int RoundingUp(int value, int divisor) =>
        (value / divisor) + (value % divisor == 0 ? 0 : 1);
}