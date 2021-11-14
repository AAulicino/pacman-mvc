using UnityEngine;

public class PinkyBehavior : BaseEnemyAIBehavior
{
    public override EnemyType EnemyType => EnemyType.Pinky;
    readonly IPinkySettings settings;

    public PinkyBehavior (
        Tile[,] map,
        IPathFinder pathFinder,
        IRandomProvider randomProvider,
        IPinkySettings settings
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
        => pathFinder.FindPath(position, GetRandomScatterPosition(settings.ScatterPosition));

    Vector2Int[] GetChaseAction (Vector2Int position, IActorModel target)
    {
        Vector2Int leadingPosition = target.Position;
        leadingPosition += target.DirectionVector * settings.LeadingTilesAheadOfPacman;

        if (target.Direction == Direction.Up)
            leadingPosition += Vector2Int.left * settings.LeadingTilesAheadOfPacman; // replicating original pacman overflow bug :)

        return pathFinder.FindPath(position, GetValidPositionCloseTo(leadingPosition));
    }

    Vector2Int[] GetFrightenedAction (Vector2Int position, IActorModel target)
    {
        Vector2Int fleeDirection = (position - target.Position) * mapMagnitude;
        return pathFinder.FindPath(position, fleeDirection);
    }
}
