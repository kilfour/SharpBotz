using System.Text.RegularExpressions;
using QuickPulse.Explains.Formatters;

namespace SharpBotz.Tests;

public class ArenaGridAttribute : CodeFormatAttribute
{
    public ArenaGridAttribute() : base(typeof(ArenaGridFormatter)) { }
}

public partial class ArenaGridFormatter : ICodeFormatter
{
    public IEnumerable<string> Format(IEnumerable<string> lines)
    {
        var sourceLines = string.Join(Environment.NewLine, lines)
            .ReplaceLineEndings("\n")
            .Split('\n');

        foreach (var line in sourceLines)
        {
            var cells = ArenaTileRegex().Matches(line)
                .Cast<Match>()
                .Select(match => FormatTile(match.Groups["name"].Value))
                .ToArray();

            if (cells.Length > 0)
                yield return string.Join(" ", cells);
        }
    }

    private static string FormatTile(string tile) =>
        tile == "Empty" ? "    " : tile;

    [GeneratedRegex(@"ArenaTile\.(?<name>\w+)")]
    private static partial Regex ArenaTileRegex();
}
