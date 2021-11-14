using System;

public class GameSessionModel : IGameSessionModel
{
    public event Action OnGameStart;
    public event Action<bool> OnGameEnded;

    public IGameModel GameModel { get; private set; }

    readonly IGameModelFactory gameModelFactory;
    readonly IMapFactory mapFactory;

    public GameSessionModel (
        IGameModelFactory gameModelFactory,
        IMapFactory mapFactory
    )
    {
        this.gameModelFactory = gameModelFactory;
        this.mapFactory = mapFactory;
    }

    public void StartNewGame ()
    {
        if (GameModel != null)
            GameModel.Dispose();

        Tile[,] tiles = mapFactory.LoadMap(0);
        GameModel = gameModelFactory.Create(tiles);
        GameModel.OnGameEnded += HandleGameEnded;

        OnGameStart?.Invoke();
        GameModel.Initialize();
    }

    void HandleGameEnded (bool victory) => OnGameEnded?.Invoke(victory);
}
