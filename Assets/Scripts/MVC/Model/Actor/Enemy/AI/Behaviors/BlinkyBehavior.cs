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
    ) : base(map, pathFinder, randomProvider)
    {
        this.settings = settings;
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
        => FindPath(position, GetRandomScatterPosition(settings.ScatterPosition));

    Vector2Int[] GetChaseAction (Vector2Int position, IActorModel target)
        => FindPath(position, target.Position);

    Vector2Int[] GetFrightenedAction (Vector2Int position, IActorModel target)
    {
        Vector2Int fleeDirection = (position - target.Position) * map.Magnitude;
        return FindPath(position, fleeDirection);
    }
}
