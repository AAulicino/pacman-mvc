using UnityEngine;

public class InkyBehavior : BaseEnemyAIBehavior
{
    public override EnemyType EnemyType => EnemyType.Inky;

    readonly IEnemyModel blinky;
    readonly IInkySettings settings;
    readonly ICollectiblesManagerModel collectiblesManager;
    readonly Bounds spawnBounds;

    public InkyBehavior (
        IMapModel map,
        IPathFinder pathFinder,
        IRandomProvider randomProvider,
        IInkySettings settings,
        ICollectiblesManagerModel collectiblesManager,
        IEnemyModel blinky
    ) : base(map, pathFinder, randomProvider, settings)
    {
        this.blinky = blinky;
        this.settings = settings;
        this.collectiblesManager = collectiblesManager;

        Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
        Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);

        for (int i = 0; i < map.EnemySpawnPoints.Count; i++)
        {
            min = Vector2Int.Min(min, map.EnemySpawnPoints[i]);
            max = Vector2Int.Max(max, map.EnemySpawnPoints[i]);
        }

        Bounds bounds = new Bounds();
        bounds.SetMinMax((Vector2)min, (Vector2)max);
        spawnBounds = bounds;
    }

    public override Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target)
    {
        if (collectiblesManager.CollectedCount < settings.CollectedRequirement)
        {
            Vector2Int randomPos = new Vector2Int(
                position.x + Random.Range((int)-spawnBounds.size.x, (int)spawnBounds.size.x),
                position.y + Random.Range((int)-spawnBounds.size.y, (int)spawnBounds.size.y)
            );
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
        Vector2Int leadingPosition = target.Position;
        leadingPosition += target.DirectionVector * settings.LeadingTilesAheadOfPacman;

        if (target.Direction == Direction.Up)
            leadingPosition += Vector2Int.left * settings.LeadingTilesAheadOfPacman; // replicating original pacman overflow bug

        leadingPosition += (leadingPosition - blinky.Position) * 2;
        return FindPath(position, GetValidPositionCloseTo(leadingPosition));
    }
}
