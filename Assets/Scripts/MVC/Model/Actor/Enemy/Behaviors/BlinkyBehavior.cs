using UnityEngine;

public class BlinkyBehavior : BaseEnemyAIBehavior
{
    public override EnemyType EnemyType => EnemyType.Blinky;

    public BlinkyBehavior (Tile[,] map, IPathFinder pathFinder) : base(map, pathFinder)
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
        Vector2Int topRightArea = new Vector2Int(
            Random.Range(mapWidth / 2, mapWidth),
            Random.Range(mapHeight / 2, mapHeight)
        );

        return pathFinder.FindPath(position, topRightArea);
    }

    Vector2Int[] GetChaseAction (Vector2Int position, IActorModel target)
        => pathFinder.FindPath(position, target.Position);

    Vector2Int[] GetFrightenedAction (Vector2Int position, IActorModel target)
    {
        Vector2Int fleeDirection = (position - target.Position) * mapMagnitude;
        return pathFinder.FindPath(position, fleeDirection);
    }
}
