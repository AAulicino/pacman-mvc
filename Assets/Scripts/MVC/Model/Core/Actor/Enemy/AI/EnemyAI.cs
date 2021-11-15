using System;
using UnityEngine;

public class EnemyAI : IEnemyAI
{
    public event Action OnActiveModeChanged;

    public EnemyType EnemyType => behavior.EnemyType;

    public Vector2Int Position { get; private set; }
    public Direction Direction { get; private set; }
    public EnemyAIMode ActiveMode { get; private set; }

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
        if (path == null || node == path.Length || ActiveMode == EnemyAIMode.Chase)
        {
            SyncActiveModeWithManager();
            RefreshPath();
        }

        if (path == null || path.Length == 0)
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
        if (ActiveMode == EnemyAIMode.Dead)
            return;

        SyncActiveModeWithManager();
        RefreshPath();
    }

    void SyncActiveModeWithManager ()
    {
        ActiveMode = modeManager.ActiveMode;
        OnActiveModeChanged?.Invoke();
    }

    void RefreshPath ()
    {
        path = behavior.GetAction(Position, ActiveMode, target);
        node = 0;
    }

    public void Die ()
    {
        ActiveMode = EnemyAIMode.Dead;
        path = behavior.GetAction(Position, ActiveMode, target);
        node = 0;
        OnActiveModeChanged?.Invoke();
    }
}
