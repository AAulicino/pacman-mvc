using UnityEngine;

public class PinkyBehavior : BaseEnemyAIBehavior
{
    const int LEADING_TILES = 2;

    public override EnemyType EnemyType => EnemyType.Pinky;

    public PinkyBehavior (Tile[,] map, IPathFinder pathFinder) : base(map, pathFinder)
    {
    }

    public override Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target)
    {
        return mode switch
        {
            EnemyAIMode.Scatter => GetScatterAction(position, target),
            EnemyAIMode.Chase => GetChaseAction(position, target),
            EnemyAIMode.Frightened => GetFrightenedAction(position, target),
            _ => throw new System.NotImplementedException()
        };
    }

    Vector2Int[] GetScatterAction (Vector2Int position, IActorModel target)
    {
        Vector2Int topLeftArea = new Vector2Int(
            Random.Range(0, mapWidth / 2),
            Random.Range(mapHeight / 2, mapHeight)
        );

        return pathFinder.FindPath(position, topLeftArea);
    }

    Vector2Int[] GetChaseAction (Vector2Int position, IActorModel target)
    {
        Vector2Int leadingPosition = target.Position + target.DirectionVector * LEADING_TILES;
        if (target.Direction == Direction.Up)
            leadingPosition += Vector2Int.left * LEADING_TILES; // replicating original pacman overflow bug :)
        return pathFinder.FindPath(position, GetValidPositionCloseTo(leadingPosition));
    }

    Vector2Int[] GetFrightenedAction (Vector2Int position, IActorModel target)
    {
        Vector2Int fleeDirection = (position - target.Position) * mapMagnitude;
        return pathFinder.FindPath(position, fleeDirection);
    }
}
