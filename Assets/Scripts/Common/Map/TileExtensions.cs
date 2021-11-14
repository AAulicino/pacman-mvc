public static class TileExtensions
{
    public static bool IsPlayerWalkable (this Tile tile)
    {
        return tile == Tile.Path
            || tile == Tile.PowerUp
            || tile == Tile.Teleport
            || tile == Tile.PlayerSpawn;
    }

    public static bool IsEnemyWalkable (this Tile tile)
        => tile != Tile.Wall && tile != Tile.Teleport;
}
