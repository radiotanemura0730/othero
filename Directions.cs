public static class Directions
{
    public static readonly int[,] AllDirections = new int[,]
    {
        {1, 0}, {-1, 0}, {0, 1}, {0, -1}, // 上下左右
        {1, 1}, {-1, -1}, {1, -1}, {-1, 1} // 斜め
    };
}