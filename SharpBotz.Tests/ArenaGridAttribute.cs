using System.Text.RegularExpressions;
using QuickPulse.Explains.Formatters;

namespace SharpBotz.Tests;

public class ArenaGridAttribute : CodeFormatAttribute
{
    public ArenaGridAttribute() : base(typeof(ArenaGridFormatter)) { }
}

public partial class ArenaGridFormatter : ICodeFormatter
{
    private static readonly Regex Tile = ArenaTyleRegex();

    public IEnumerable<string> Format(IEnumerable<string> lines)
    {
        var columns = lines
            .Select(line => Tile.Matches(line)
                .Cast<Match>()
                .Select(match => match.Groups["name"].Value)
                .ToArray())
            .Where(column => column.Length > 0)
            .ToArray();

        if (columns.Length == 0)
            yield break;

        var height = columns[0].Length;
        if (columns.Any(column => column.Length != height))
            throw new FormatException("All arena grid columns must have the same height.");

        for (var y = 0; y < height; y++)
        {
            yield return string.Join(
                " ",
                columns.Select(column => FormatTile(column[y])));
        }
    }

    private static string FormatTile(string tile) =>
        tile == "Empty" ? "    " : tile;

    [GeneratedRegex(@"ArenaTileType\.(?<name>\w+)")]
    private static partial Regex ArenaTyleRegex();
}
