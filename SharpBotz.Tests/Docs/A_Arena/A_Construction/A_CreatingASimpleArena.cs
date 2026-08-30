using QuickPulse.Explains;
using SharpBotz.Arenas;

namespace SharpBotz.Tests.Docs.A_Arena.A_Construction;

[DocFile]
public class A_CreatingASimpleArena
{
    [Fact]
    [DocContent(
"""
Construct an `Arena` by calling the static `Create` method,
which takes an `ArenaWidth` and an `ArenaHeight` as arguments: 
"""
    )]
    [DocExample(typeof(A_CreatingASimpleArena), nameof(GetArena))]
    [DocContent(
"""
This creates a 3 by 3 grid.  
The outer tiles are set up as *Walls*
"""
    )]
    [DocExample(typeof(A_CreatingASimpleArena), nameof(ExpectedGrid), "text")]
    public void Construction()
    {
        var arena = GetArena();
        var expectedGrid = ExpectedGrid();
        AssertGrid.Equal(expectedGrid, arena);
    }

    [CodeSnippet]
    private static Arena GetArena() =>
        Arena.Create(
            ArenaWidth.Is(3),
            ArenaHeight.Is(3));

    [CodeSnippet]
    [ArenaGrid]
    private static ArenaTileType[,] ExpectedGrid() =>
        new ArenaTileType[,]
        {
            { ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Wall },
            { ArenaTileType.Wall, ArenaTileType.Empty, ArenaTileType.Wall },
            { ArenaTileType.Wall, ArenaTileType.Wall, ArenaTileType.Wall }
        };

    [Theory]
    [InlineData(0, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [DocContent("Both `ArenaWidth` and `ArenaHeight` must be greater than 2")]
    public void MinimumWidthAndHeight(int value, bool throws)
    {
        if (throws)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ArenaWidth.Is(value));
            Assert.Throws<ArgumentOutOfRangeException>(() => ArenaHeight.Is(value));
            return;
        }
        Assert.Equal(value, ArenaWidth.Is(value).Value);
        Assert.Equal(value, ArenaHeight.Is(value).Value);
    }
}
