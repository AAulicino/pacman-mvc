using System;

public interface IGameSessionModel
{
    event Action<bool> OnGameEnded;
    event Action OnGameStart;

    IGameModel GameModel { get; }

    void StartNewGame ();
}
