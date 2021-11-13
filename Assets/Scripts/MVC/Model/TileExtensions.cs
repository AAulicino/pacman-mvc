public static class TileExtensions
{
    public static bool IsPlayerWalkable (this Tile tile)
        => tile == Tile.Path || tile == Tile.Teleport || tile == Tile.PowerUp;

    public static bool IsEnemyWalkable (this Tile tile)
        => tile != Tile.Wall && tile != Tile.Teleport;
}