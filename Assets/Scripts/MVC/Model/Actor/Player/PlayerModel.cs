using System;
using System.Collections;
using UnityEngine;

public class PlayerModel : IPlayerModel
{
    public event Action OnTeleport;
    public event Action OnPositionChanged;
    public event Action OnDirectionChanged;
    public event Action<bool> OnEnableChange;

    public Vector2Int Position { get; private set; } = Vector2Int.one;
    public Vector2Int DirectionVector { get; private set; }
    public Direction Direction { get; private set; }
    public bool HasPowerUp { get; private set; }

    public float MovementTime => settings.MovementTime;

    readonly IMapModel map;
    readonly ICoroutineRunner runner;
    readonly IActorSettings settings;
    readonly IInputProvider input;
    readonly ITimeProvider time;

    Coroutine moveCoroutine;
    Direction nextMovement = Direction.Left;
    bool skipWait;

    public PlayerModel (
        IMapModel map,
        ICoroutineRunner runner,
        IActorSettings settings,
        IInputProvider input,
        ITimeProvider timeProvider
    )
    {
        this.map = map;
        this.runner = runner;
        this.settings = settings;
        this.input = input;
        this.time = timeProvider;

        Position = map.PlayerSpawnPoint;
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

    IEnumerator MoveRoutine ()
    {
        float delta = 0;
        while (true)
        {
            delta += time.DeltaTime;

            if (input.GetKey(KeyCode.UpArrow))
                nextMovement = Direction.Up;
            else if (input.GetKey(KeyCode.DownArrow))
                nextMovement = Direction.Down;
            else if (input.GetKey(KeyCode.LeftArrow))
                nextMovement = Direction.Left;
            else if (input.GetKey(KeyCode.RightArrow))
                nextMovement = Direction.Right;

            if (skipWait || delta >= settings.MovementTime)
            {
                delta = 0;
                skipWait = false;
                Move(nextMovement);
            }
            yield return null;
        }
    }

    void Move (Direction direction)
    {
        Vector2Int movementDirectionVector = GetMovementDirection(direction);
        Vector2Int newPosition = Position + movementDirectionVector;

        if (!IsMovementValid(newPosition))
        {
            // preserve previous direction if direction change not valid
            nextMovement = Direction;
            movementDirectionVector = GetMovementDirection(Direction);
            newPosition = Position + movementDirectionVector;

            if (!IsMovementValid(newPosition))
                return;
        }

        HandleDirection(movementDirectionVector);

        if (HandleTeleport(newPosition))
        {
            skipWait = true;
            return;
        }

        Position = newPosition;
        OnPositionChanged?.Invoke();
    }

    bool IsMovementValid (Vector2Int position)
        => map.InBounds(position) && map[position].IsPlayerWalkable();

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
        Disable();
    }

    public void PowerUp ()
    {
        HasPowerUp = true;
    }

    void HandleDirection (Vector2Int movementDirectionVector)
    {
        if (Direction != nextMovement)
        {
            Direction = nextMovement;
            DirectionVector = movementDirectionVector;
            OnDirectionChanged?.Invoke();
        }
    }

    bool HandleTeleport (Vector2Int position)
    {
        if (map[position] == Tile.Teleport)
        {
            foreach (Vector2Int pos in map.TeleportPositions)
            {
                if (pos != position)
                {
                    Position = pos;
                    OnTeleport?.Invoke();
                    return true;
                }
            }
        }
        return false;
    }

    public void Dispose ()
    {
        Disable();
    }
}
