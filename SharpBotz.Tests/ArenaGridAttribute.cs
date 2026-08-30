using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using QuickPulse.Explains.Formatters;

namespace SharpBotz.Tests;

public class ArenaGridAttribute : CodeFormatAttribute
{
    public ArenaGridAttribute() : base(typeof(ArenaGridFormatter)) { }
}

public class ArenaGridFormatter : ICodeFormatter
{
    public IEnumerable<string> Format(IEnumerable<string> lines)
    {
        var source = string.Join(Environment.NewLine, lines).Trim().TrimEnd(';');
        var array = SyntaxFactory.ParseExpression(source) as ArrayCreationExpressionSyntax
            ?? throw new FormatException("The arena grid must be a rectangular array initializer.");
        var initializer = array.Initializer
            ?? throw new FormatException("The arena grid must have an initializer.");

        var columns = initializer.Expressions
            .Select(ReadColumn)
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

    private static string[] ReadColumn(ExpressionSyntax expression)
    {
        if (expression is not InitializerExpressionSyntax column)
            throw new FormatException("Each arena grid column must be an initializer.");

        return [.. column.Expressions.Select(ReadTile)];
    }

    private static string ReadTile(ExpressionSyntax expression) =>
        expression is MemberAccessExpressionSyntax tile
            ? tile.Name.Identifier.ValueText
            : throw new FormatException("Each arena grid cell must be an ArenaTileType value.");
}
