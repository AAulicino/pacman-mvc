using System;
using System.Collections;
using UnityEngine;

public class EnemyModel : IEnemyModel
{
    public event Action OnPositionChanged;
    public event Action OnDirectionChanged;
    public event Action<bool> OnEnableChange;
    public event Action OnActiveModeChanged;

    public Vector2Int Position => ai.Position;
    public Vector2Int DirectionVector { get; private set; }

    public Direction Direction { get; private set; }

    public EnemyType EnemyType => ai.EnemyType;
    public EnemyAIMode ActiveMode => ai.ActiveMode;

    public float MovementTime { get; private set; }

    readonly IEnemyAI ai;
    readonly IActorSettings settings;
    readonly ICoroutineRunner runner;
    Coroutine moveCoroutine;

    public EnemyModel (IEnemyAI ai, IActorSettings settings, ICoroutineRunner runner)
    {
        this.ai = ai;
        this.settings = settings;
        this.runner = runner;

        ai.OnActiveModeChanged += HandleActiveModeChanged;
        HandleActiveModeChanged();
    }

    public void Enable ()
    {
        moveCoroutine = runner.StartCoroutine(MoveRoutine());
        OnEnableChange?.Invoke(true);
    }

    public void Disable ()
    {
        if (moveCoroutine != null)
            runner.StopCoroutine(moveCoroutine);
    }

    void HandleActiveModeChanged ()
    {
        if (ai.ActiveMode == EnemyAIMode.Frightened)
            MovementTime = settings.FrightenedMoveTime;
        else
            MovementTime = settings.MovementTime;
        OnActiveModeChanged?.Invoke();
    }

    IEnumerator MoveRoutine ()
    {
        float delta = 0;
        while (true)
        {
            delta += Time.deltaTime;

            if (delta >= MovementTime)
            {
                delta = 0;
                ai.Advance();
                Move(ai.Direction);
            }
            yield return null;
        }
    }

    void Move (Direction direction)
    {
        Vector2Int movementDirectionVector = GetMovementDirection(Direction);
        Vector2Int newPosition = Position + movementDirectionVector;

        if (Direction != direction)
        {
            Direction = direction;
            DirectionVector = movementDirectionVector;
            OnDirectionChanged?.Invoke();
        }

        OnPositionChanged?.Invoke();
    }

    Vector2Int GetMovementDirection (Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector2Int.up,
            Direction.Down => Vector2Int.down,
            Direction.Left => Vector2Int.left,
            Direction.Right => Vector2Int.right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };
    }

    public void Die ()
    {
        ai.Die();
    }

    public void Dispose ()
    {
        Disable();
    }
}
