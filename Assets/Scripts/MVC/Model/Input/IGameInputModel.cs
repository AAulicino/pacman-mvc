using System;

public interface IGameInputModel
{
    event Action OnDirectionChanged;

    Direction GetDirection ();
    void SetDirection (Direction direction);
}
