using System;
using UnityEngine;

public class EnemyAI : IEnemyAI
{
    public event Action OnActiveModeChanged
    {
        add => modeManager.OnActiveModeChanged += value;
        remove => modeManager.OnActiveModeChanged -= value;
    }

    public EnemyType EnemyType => behavior.EnemyType;

    public Vector2Int Position { get; private set; }
    public Direction Direction { get; private set; }
    public EnemyAIMode ActiveMode => modeManager.ActiveMode;

    readonly IEnemyAIModeManagerModel modeManager;
    readonly IActorModel target;
    readonly IEnemyAIBehavior behavior;

    Vector2Int[] path;
    int node;
    EnemyAIMode currentMode;

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
        if (currentMode != EnemyAIMode.Dead && currentMode != modeManager.ActiveMode)
            RefreshPath();
        else if (path == null || node == path.Length || modeManager.ActiveMode == EnemyAIMode.Chase)
            RefreshPath();

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

    void HandleActiveModeChanged () => RefreshPath();

    void RefreshPath ()
    {
        path = behavior.GetAction(Position, modeManager.ActiveMode, target);
        node = 0;
        currentMode = modeManager.ActiveMode;
    }

    public void Die ()
    {
        currentMode = EnemyAIMode.Dead;
        path = behavior.GetAction(Position, currentMode, target);
        node = 0;
    }
}
