namespace SharpBotz.Spectre;

public static class BotPalette
{
    public static (string Foreground, string Background) GetColors(int botIndex) =>
        (botIndex % 10) switch
        {
            0 => ("deepskyblue1", "rgb(0,30,45)"),
            1 => ("magenta1", "rgb(45,0,35)"),
            2 => ("green1", "rgb(0,40,20)"),
            3 => ("yellow1", "rgb(45,35,0)"),
            4 => ("orange1", "rgb(45,20,0)"),
            5 => ("cyan1", "rgb(0,40,40)"),
            6 => ("rgb(255,80,80)", "rgb(45,0,0)"),
            7 => ("rgb(190,110,255)", "rgb(25,10,45)"),
            8 => ("rgb(80,255,155)", "rgb(0,45,25)"),
            _ => ("rgb(235,235,235)", "rgb(35,35,35)"),
        };
}
