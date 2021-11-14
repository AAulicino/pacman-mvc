using UnityEngine;

public class InkyBehavior : BaseEnemyAIBehavior
{
    public override EnemyType EnemyType => EnemyType.Inky;

    readonly IEnemyModel blinky;
    readonly IInkySettings settings;
    readonly ICollectiblesManagerModel collectiblesManager;

    public InkyBehavior (
        IMapModel map,
        IPathFinder pathFinder,
        IRandomProvider randomProvider,
        ICollectiblesManagerModel collectiblesManager,
        IEnemyModel blinky,
        IInkySettings settings
    ) : base(map, pathFinder, randomProvider)
    {
        this.blinky = blinky;
        this.settings = settings;
        this.collectiblesManager = collectiblesManager;
    }

    public override Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target)
    {
        if (collectiblesManager.CollectedCount < settings.CollectedRequirement)
        {
            Vector2Int randomPos = new Vector2Int(position.x + Random.Range(-3, 4), position.y);
            return FindPath(
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
        => FindPath(position, GetRandomScatterPosition(settings.ScatterPosition));

    Vector2Int[] GetChaseAction (Vector2Int position, IActorModel target)
    {
        Vector2Int leadingPosition = target.Position;
        leadingPosition += target.DirectionVector * settings.LeadingTilesAheadOfPacman;

        if (target.Direction == Direction.Up)
            leadingPosition += Vector2Int.left * settings.LeadingTilesAheadOfPacman; // replicating original pacman overflow bug

        leadingPosition += (leadingPosition - blinky.Position) * 2;
        return FindPath(position, GetValidPositionCloseTo(leadingPosition));
    }

    Vector2Int[] GetFrightenedAction (Vector2Int position, IActorModel target)
    {
        Vector2Int fleeDirection = (position - target.Position) * map.Magnitude;
        return FindPath(position, fleeDirection);
    }
}
