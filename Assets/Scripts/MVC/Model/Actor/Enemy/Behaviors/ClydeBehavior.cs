using UnityEngine;

public class ClydeBehavior : BaseEnemyAIBehavior
{
    const float COLLECTED_REQUIREMENT_RATIO = 0.3f;
    const int SQR_DISABLE_CHASE_DISTANCE = 4 * 4;

    public override EnemyType EnemyType => EnemyType.Clyde;

    readonly ICollectiblesManagerModel collectiblesManager;

    public ClydeBehavior (
        Tile[,] map,
        IPathFinder pathFinder,
        ICollectiblesManagerModel collectiblesManager
    ) : base(map, pathFinder)
    {
        this.collectiblesManager = collectiblesManager;
    }

    public override Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target)
    {
        float ratio = collectiblesManager.CollectedCount
            / (float)collectiblesManager.TotalCollectibles;

        if (ratio < COLLECTED_REQUIREMENT_RATIO)
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
        Vector2Int bottomLeftArea = new Vector2Int(
            Random.Range(0, mapWidth / 2),
            Random.Range(0, mapHeight / 2)
        );

        return pathFinder.FindPath(position, bottomLeftArea);
    }

    Vector2Int[] GetChaseAction (Vector2Int position, IActorModel target)
    {
        if ((target.Position - position).sqrMagnitude > SQR_DISABLE_CHASE_DISTANCE)
            return pathFinder.FindPath(position, target.Position);

        return GetScatterAction(position, target);
    }

    Vector2Int[] GetFrightenedAction (Vector2Int position, IActorModel target)
    {
        Vector2Int fleeDirection = (position - target.Position) * mapMagnitude;
        return pathFinder.FindPath(position, fleeDirection);
    }
}
