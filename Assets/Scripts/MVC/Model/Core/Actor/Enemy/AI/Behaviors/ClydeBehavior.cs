using UnityEngine;

public class ClydeBehavior : BaseEnemyAIBehavior
{
    public override EnemyType EnemyType => EnemyType.Clyde;

    readonly ICollectiblesManagerModel collectiblesManager;
    readonly IClydeSettings settings;

    readonly int sqrChaseDistance;

    public ClydeBehavior (
        IMapModel map,
        IPathFinder pathFinder,
        IRandomProvider randomProvider,
        IClydeSettings settings,
        ICollectiblesManagerModel collectiblesManager
    ) : base(map, pathFinder, randomProvider, settings)
    {
        this.collectiblesManager = collectiblesManager;
        this.settings = settings;
        sqrChaseDistance = settings.DisableChaseDistance * settings.DisableChaseDistance;
    }

    public override Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target)
    {
        float ratio = collectiblesManager.CollectedCount
            / (float)collectiblesManager.TotalCollectibles;

        if (ratio < settings.CollectedRequirementRatio)
        {
            Vector2Int randomPos = new Vector2Int(position.x + Random.Range(-3, 4), position.y);
            return FindPath(
                position,
                GetValidPositionCloseTo(randomPos, x => x == Tile.EnemySpawn)
            );
        }

        return mode switch
        {
            EnemyAIMode.Scatter => GetDefaultScatterAction(position, target),
            EnemyAIMode.Chase => GetChaseAction(position, target),
            EnemyAIMode.Frightened => GetDefaultFrightenedAction(position, target),
            EnemyAIMode.Dead => GetDefaultDeadAction(position, target),
            _ => throw new System.NotImplementedException()
        };
    }

    Vector2Int[] GetChaseAction (Vector2Int position, IActorModel target)
    {
        if ((target.Position - position).sqrMagnitude > sqrChaseDistance)
            return FindPath(position, target.Position);

        return GetDefaultScatterAction(position, target);
    }
}
