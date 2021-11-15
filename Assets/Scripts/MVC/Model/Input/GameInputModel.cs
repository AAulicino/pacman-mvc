using System;
using UnityEngine;

public class GameInputModel : IGameInputModel
{
    public event Action OnDirectionChanged;

    readonly IInputProvider inputProvider;

    Direction direction = Direction.Left;

    public GameInputModel (IInputProvider inputProvider)
    {
        this.inputProvider = inputProvider;
    }

    public Direction GetDirection ()
    {
        if (inputProvider.GetKey(KeyCode.UpArrow))
            SetDirection(Direction.Up);
        else if (inputProvider.GetKey(KeyCode.DownArrow))
            SetDirection(Direction.Down);
        else if (inputProvider.GetKey(KeyCode.LeftArrow))
            SetDirection(Direction.Left);
        else if (inputProvider.GetKey(KeyCode.RightArrow))
            SetDirection(Direction.Right);

        return direction;
    }

    public void SetDirection (Direction direction)
    {
        if (this.direction != direction)
        {
            this.direction = direction;
            OnDirectionChanged?.Invoke();
        }
    }
}
