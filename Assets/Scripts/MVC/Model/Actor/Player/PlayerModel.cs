using System;
using System.Collections;
using UnityEngine;

public class PlayerModel : IPlayerModel
{
    public event Action OnPositionChanged;
    public event Action OnDirectionChanged;
    public event Action<bool> OnEnableChange;

    public Vector2Int Position { get; private set; } = Vector2Int.one;
    public Direction Direction { get; private set; }
    public float MovementTime => settings.MovementTime;

    readonly Tile[,] map;
    readonly ICoroutineRunner runner;
    readonly IActorSettings settings;

    Coroutine moveCoroutine;
    Direction nextMovement;

    public PlayerModel (Tile[,] map, ICoroutineRunner runner, IActorSettings settings)
    {
        this.map = map;
        this.runner = runner;
        this.settings = settings;
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
            delta += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                nextMovement = Direction.Up;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                nextMovement = Direction.Down;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                nextMovement = Direction.Left;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                nextMovement = Direction.Right;

            if (delta >= MovementTime)
            {
                delta = 0;
                Move(nextMovement);
            }
            yield return null;
        }
    }

    void Move (Direction direction)
    {
        Vector2Int newPosition = Position + GetMovementDirection(direction);

        if (!IsMovementValid(newPosition))
        {
            // preserve previous direction if direction change not valid
            nextMovement = Direction;
            newPosition = Position + GetMovementDirection(Direction);
            if (!IsMovementValid(newPosition))
                return;
        }

        if (Direction != nextMovement)
        {
            Direction = nextMovement;
            OnDirectionChanged?.Invoke();
        }

        Position = newPosition;
        OnPositionChanged?.Invoke();
    }

    bool IsMovementValid (Vector2Int position)
    {
        return position.x > 0 && position.x < map.GetLength(0)
            && position.y > 0 && position.y < map.GetLength(1)
            && map[position.x, position.y].IsPlayerWalkable();
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
        throw new NotImplementedException();
    }
}
