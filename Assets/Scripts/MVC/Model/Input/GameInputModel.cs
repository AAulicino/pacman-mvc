using UnityEngine;

public class GameInputModel : IGameInputModel
{
    readonly IInputProvider inputProvider;

    Direction direction = Direction.Left;

    public GameInputModel (IInputProvider inputProvider)
    {
        this.inputProvider = inputProvider;
    }

    public Direction GetDirection ()
    {
        if (inputProvider.GetKey(KeyCode.UpArrow))
            return Direction.Up;
        else if (inputProvider.GetKey(KeyCode.DownArrow))
            return Direction.Down;
        else if (inputProvider.GetKey(KeyCode.LeftArrow))
            return Direction.Left;
        else if (inputProvider.GetKey(KeyCode.RightArrow))
            return Direction.Right;

        return direction;
    }

    public void SetDirection (Direction direction) => this.direction = direction;
}
