using UnityEngine;

public class BlinkyBehavior : BaseEnemyAIBehavior
{
    public override EnemyType EnemyType => EnemyType.Blinky;

    readonly IBlinkySettings settings;

    public BlinkyBehavior (
        IMapModel map,
        IPathFinder pathFinder,
        IRandomProvider randomProvider,
        IBlinkySettings settings
    ) : base(map, pathFinder, randomProvider, settings)
    {
        this.settings = settings;
    }

    public override Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target)
    {
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
        => FindPath(position, target.Position);
}
