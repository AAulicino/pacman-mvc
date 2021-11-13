using UnityEngine;

public class EnemyAI : IEnemyAI
{
    public EnemyType EnemyType => behavior.EnemyType;

    public Vector2Int Position { get; private set; }
    public Direction Direction { get; private set; }

    readonly IEnemyAIModeManagerModel modeManager;
    readonly IActorModel target;
    readonly IEnemyAIBehavior behavior;

    Vector2Int[] path;
    int node;

    public EnemyAI (
        Vector2Int startingPosition,
        IEnemyAIModeManagerModel modeManager,
        IActorModel target,
        IEnemyAIBehavior behavior
    )
    {
        Position = startingPosition;
        this.modeManager = modeManager;
        this.target = target;
        this.behavior = behavior;
    }

    public void Initialize ()
    {
        modeManager.OnActiveModeChanged += HandleActiveModeChanged;
    }

    public void Advance ()
    {
        if (path == null)
            return;

        Vector2Int previousPosition = Position;
        Position = path[node++];

        Vector2Int dir = previousPosition - Position;
        if (dir.x > 0)
            Direction = Direction.Right;
        else if (dir.x < 0)
            Direction = Direction.Left;
        else if (dir.y > 0)
            Direction = Direction.Up;
        else if (dir.y < 0)
            Direction = Direction.Down;
    }

    void HandleActiveModeChanged ()
    {
        path = behavior.GetAction(Position, modeManager.ActiveMode, target);
        node = 0;
    }

    public void Dispose ()
    {
        modeManager.OnActiveModeChanged -= HandleActiveModeChanged;
    }
}
