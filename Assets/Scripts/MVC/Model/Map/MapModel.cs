using System.Collections.Generic;
using UnityEngine;

public class MapModel : IMapModel
{
    public Tile this[int x, int y] => map[x, y];
    public Tile this[Vector2Int pos] => map[pos.x, pos.y];

    public int Width { get; }
    public int Height { get; }
    public int Magnitude { get; }

    public IReadOnlyList<Vector2Int> TeleportPositions { get; }
    public IReadOnlyList<Vector2Int> EnemySpawnPoints { get; }
    public Vector2Int PlayerSpawnPoint { get; }

    readonly Tile[,] map;

    public MapModel (Tile[,] map)
    {
        this.map = map;
        Width = map.GetLength(0);
        Height = map.GetLength(1);

        List<Vector2Int> teleportPositions = new List<Vector2Int>();
        List<Vector2Int> enemySpawnPoints = new List<Vector2Int>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tile tile = map[x, y];
                switch (tile)
                {
                    case Tile.Teleport: teleportPositions.Add(new Vector2Int(x, y)); break;
                    case Tile.EnemySpawn: enemySpawnPoints.Add(new Vector2Int(x, y)); break;
                    case Tile.PlayerSpawn: PlayerSpawnPoint = new Vector2Int(x, y); break;
                }
            }
        }

        TeleportPositions = teleportPositions;
        EnemySpawnPoints = enemySpawnPoints;
        Magnitude = (int)Mathf.Sqrt(Width * Width + Height * Height);
    }

    public bool InBounds (Vector2Int position) => InBounds(position.x, position.y);

    public bool InBounds (int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
}
