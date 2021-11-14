using System.Collections.Generic;
using UnityEngine;

public interface IMapModel
{
    Tile this[Vector2Int pos] { get; }
    Tile this[int x, int y] { get; }

    int Width { get; }
    int Height { get; }
    int Magnitude { get; }

    IReadOnlyList<Vector2Int> TeleportPositions { get; }
    IReadOnlyList<Vector2Int> EnemySpawnPoints { get; }
    Vector2Int PlayerSpawnPoint { get; }

    bool InBounds (Vector2Int position);
    bool InBounds (int x, int y);
}
