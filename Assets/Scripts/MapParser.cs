using System;

public static class MapParser
{
    public static Tile[,] Parse (string mapString)
    {
        string[] rows = mapString.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int height = rows.Length;
        int width = rows[0].Length;

        Tile[,] map = new Tile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                map[x, height - 1 - y] = (Tile)int.Parse(rows[y][x].ToString());
        }
        return map;
    }
}
