using System.Diagnostics;
using SharpBotz.Arenas;

namespace SharpBotz.Tests;

public static class AssertGrid
{
    [StackTraceHidden]
    public static void Equal(ArenaTile[,] expectedGrid, Arena arena)
    {
        Assert.Equal(expectedGrid.GetLength(1), arena.Width);
        Assert.Equal(expectedGrid.GetLength(0), arena.Height);
        var grid = arena.GetGrid();
        for (int y = 0; y < arena.Height; y++)
        {
            for (int x = 0; x < arena.Width; x++)
            {
                Assert.True(
                    expectedGrid[y, x] == grid[x, y],
$"""

-------------------------------
Tile mismatch at ({x}, {y}). 
    Expected: {expectedGrid[y, x]}. 
    Actual: {arena[x, y]}.
-------------------------------
""");
            }
        }
    }
}
