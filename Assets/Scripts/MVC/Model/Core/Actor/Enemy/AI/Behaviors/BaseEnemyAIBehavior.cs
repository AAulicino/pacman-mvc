using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAIBehavior : IEnemyAIBehavior
{
    public abstract EnemyType EnemyType { get; }

    protected readonly IMapModel map;
    protected readonly IPathFinder pathFinder;

    protected readonly IRandomProvider random;

    protected BaseEnemyAIBehavior (IMapModel map, IPathFinder pathFinder, IRandomProvider random)
    {
        this.map = map;
        this.pathFinder = pathFinder;
        this.random = random;
    }

    public abstract Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target);

    protected Vector2Int[] GetDefaultDeadAction (Vector2Int position, IActorModel target)
    {
        int rnd = random.Range(0, map.EnemySpawnPoints.Count);
        return FindPath(position, map.EnemySpawnPoints[rnd]);
    }

    protected Vector2Int[] GetDefaultFrightenedAction (Vector2Int position, IActorModel target)
    {
        Vector2Int fleeDirection = (position - target.Position) * map.Magnitude;
        return FindPath(position, GetValidPositionCloseTo(fleeDirection));
    }

    protected Vector2Int GetRandomScatterPosition (ScatterPosition position)
    {
        return GetValidPositionCloseTo(position switch
        {
            ScatterPosition.TopLeft => new Vector2Int(
                (int)random.Range(0, map.Width / 2),
                (int)random.Range(map.Height / 2, map.Height)
            ),
            ScatterPosition.TopRight => new Vector2Int(
                (int)random.Range(map.Width / 2, map.Width),
                (int)random.Range(map.Height / 2, map.Height)
            ),
            ScatterPosition.BottomLeft => new Vector2Int(
                (int)random.Range(0, map.Width / 2),
                (int)random.Range(0, map.Height / 2)
            ),
            ScatterPosition.BottomRight => new Vector2Int(
                (int)random.Range(map.Width / 2, map.Width),
                (int)random.Range(0, map.Height / 2)
            ),
            _ => throw new NotImplementedException(),
        });
    }

    protected Vector2Int[] FindPath (Vector2Int start, Vector2Int goal)
        => pathFinder.FindPath(start, goal, TileExtensions.IsEnemyWalkable);

    protected Vector2Int GetValidPositionCloseTo (Vector2Int pos)
        => GetValidPositionCloseTo(pos, TileExtensions.IsEnemyWalkable);

    protected Vector2Int GetValidPositionCloseTo (Vector2Int pos, Func<Tile, bool> validTilePredicate)
    {
        if (pos.x >= map.Width)
            return GetValidPositionCloseTo(new Vector2Int(map.Width - 1, pos.y), validTilePredicate);
        else if (pos.x < 0)
            return GetValidPositionCloseTo(new Vector2Int(0, pos.y), validTilePredicate);
        else if (pos.y >= map.Height)
            return GetValidPositionCloseTo(new Vector2Int(pos.x, map.Height - 1), validTilePredicate);
        else if (pos.y < 0)
            return GetValidPositionCloseTo(new Vector2Int(pos.x, 0), validTilePredicate);

        Tile startTile = map[pos.x, pos.y];

        if (validTilePredicate(startTile))
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
                if (validTilePredicate(map[neighbor.x, neighbor.y]))
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

        if (map.InBounds(x + 1, y))
            yield return new Vector2Int(x + 1, y);

        if (map.InBounds(x + 1, y + 1))
            yield return new Vector2Int(x + 1, y + 1);

        if (map.InBounds(x + 1, y - 1))
            yield return new Vector2Int(x + 1, y - 1);

        if (map.InBounds(x - 1, y))
            yield return new Vector2Int(x - 1, y);

        if (map.InBounds(x - 1, y - 1))
            yield return new Vector2Int(x - 1, y - 1);

        if (map.InBounds(x - 1, y + 1))
            yield return new Vector2Int(x - 1, y + 1);

        if (map.InBounds(x, y + 1))
            yield return new Vector2Int(x, y + 1);

        if (map.InBounds(x, y - 1))
            yield return new Vector2Int(x, y - 1);
    }
}
