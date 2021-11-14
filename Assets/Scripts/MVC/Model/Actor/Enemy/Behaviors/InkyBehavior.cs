using UnityEngine;

public class InkyBehavior : BaseEnemyAIBehavior
{
    const int LEADING_TILES = 1;
    const int COLLECTED_REQUIREMENT = 30;

    public override EnemyType EnemyType => EnemyType.Inky;

    readonly IEnemyModel blinky;
    readonly ICollectiblesManagerModel collectiblesManager;

    public InkyBehavior (
        Tile[,] map,
        IPathFinder pathFinder,
        IEnemyModel blinky,
        ICollectiblesManagerModel collectiblesManager
    ) : base(map, pathFinder)
    {
        this.blinky = blinky;
        this.collectiblesManager = collectiblesManager;
    }

    public override Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target)
    {
        if (collectiblesManager.CollectedCount < COLLECTED_REQUIREMENT)
        {
            Vector2Int randomPos = new Vector2Int(position.x + Random.Range(-3, 4), position.y);
            return pathFinder.FindPath(
                position,
                GetValidPositionCloseTo(randomPos, x => x == Tile.EnemySpawn)
            );
        }

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
        Vector2Int bottomRightArea = new Vector2Int(
            Random.Range(mapWidth / 2, mapWidth),
            Random.Range(0, mapHeight / 2)
        );

        return pathFinder.FindPath(position, bottomRightArea);
    }

    Vector2Int[] GetChaseAction (Vector2Int position, IActorModel target)
    {
        Vector2Int leadingPosition = target.Position + target.DirectionVector * LEADING_TILES;

        if (target.Direction == Direction.Up)
            leadingPosition += Vector2Int.left * LEADING_TILES; // replicating original pacman overflow bug :)

        leadingPosition += (leadingPosition - blinky.Position) * 2;
        return pathFinder.FindPath(position, GetValidPositionCloseTo(leadingPosition));
    }

    Vector2Int[] GetFrightenedAction (Vector2Int position, IActorModel target)
    {
        Vector2Int fleeDirection = (position - target.Position) * mapMagnitude;
        return pathFinder.FindPath(position, fleeDirection);
    }
}
