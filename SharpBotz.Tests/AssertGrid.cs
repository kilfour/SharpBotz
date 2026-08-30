using System.Diagnostics;
using SharpBotz.Arenas;

namespace SharpBotz.Tests;

public static class AssertGrid
{
    [StackTraceHidden]
    public static void Equal(ArenaTileType[,] expectedGrid, Arena arena)
    {
        Assert.Equal(expectedGrid.GetLength(0), arena.Width);
        Assert.Equal(expectedGrid.GetLength(1), arena.Height);

        for (int x = 0; x < arena.Width; x++)
        {
            for (int y = 0; y < arena.Height; y++)
            {
                Assert.True(
                    expectedGrid[x, y] == arena[x, y],
$"""

-------------------------------
Tile mismatch at ({x}, {y}). 
    Expected: {expectedGrid[x, y]}. 
    Actual: {arena[x, y]}.
-------------------------------
""");
            }
        }
    }
}
