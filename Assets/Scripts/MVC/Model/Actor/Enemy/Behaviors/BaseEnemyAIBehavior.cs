using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAIBehavior : IEnemyAIBehavior
{
    public abstract EnemyType EnemyType { get; }

    protected readonly Tile[,] map;
    protected readonly IPathFinder pathFinder;

    protected readonly int mapWidth;
    protected readonly int mapHeight;
    protected readonly int mapMagnitude;

    protected BaseEnemyAIBehavior (Tile[,] map, IPathFinder pathFinder)
    {
        this.map = map;
        this.pathFinder = pathFinder;

        mapWidth = map.GetLength(0);
        mapHeight = map.GetLength(1);
        mapMagnitude = (int)Mathf.Sqrt(mapWidth * mapWidth + mapHeight * mapHeight);
    }


    public abstract Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target);

    protected Vector2Int GetValidPositionCloseTo (Vector2Int pos)
    {
        if (pos.x > mapWidth)
            return GetValidPositionCloseTo(new Vector2Int(mapWidth - 1, pos.y));
        else if (pos.x < 0)
            return GetValidPositionCloseTo(new Vector2Int(0, pos.y));
        else if (pos.y > mapHeight)
            return GetValidPositionCloseTo(new Vector2Int(pos.x, mapHeight - 1));
        else if (pos.y < 0)
            return GetValidPositionCloseTo(new Vector2Int(pos.x, 0));

        Tile startTile = map[pos.x, pos.y];

        if (startTile.IsEnemyWalkable())
            return pos;

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(pos);

        while (queue.Count > 0)
        {
            Vector2Int currentPos = queue.Dequeue();

            if (!visited.Add(currentPos))
                continue;

            foreach (Vector2Int neighbor in GetValidMapNeighbors(currentPos))
            {
                if (map[neighbor.x, neighbor.y].IsEnemyWalkable())
                    return neighbor;

                if (!visited.Contains(neighbor))
                    queue.Enqueue(neighbor);
            }
        }

        return default;
    }

    IEnumerable<Vector2Int> GetValidMapNeighbors (Vector2Int position)
    {
        int x = position.x;
        int y = position.y;

        if (IsInBounds(x + 1, y))
            yield return new Vector2Int(x + 1, y);

        if (IsInBounds(x + 1, y + 1))
            yield return new Vector2Int(x + 1, y + 1);

        if (IsInBounds(x + 1, y - 1))
            yield return new Vector2Int(x + 1, y - 1);

        if (IsInBounds(x - 1, y))
            yield return new Vector2Int(x - 1, y);

        if (IsInBounds(x - 1, y - 1))
            yield return new Vector2Int(x - 1, y - 1);

        if (IsInBounds(x - 1, y + 1))
            yield return new Vector2Int(x - 1, y + 1);

        if (IsInBounds(x, y + 1))
            yield return new Vector2Int(x, y + 1);

        if (IsInBounds(x, y - 1))
            yield return new Vector2Int(x, y - 1);
    }

    bool IsInBounds (int x, int y)
        => x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
}
