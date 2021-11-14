public class GameSessionController
{
    readonly IGameSessionModel model;
    readonly GameSessionView view;
    readonly MapTileSpriteDatabase tileSpriteDatabase;

    GameController gameController;

    public GameSessionController (
        IGameSessionModel model,
        GameSessionView view,
        MapTileSpriteDatabase tileSpriteDatabase
    )
    {
        this.model = model;
        this.view = view;
        this.tileSpriteDatabase = tileSpriteDatabase;

        model.OnGameStart += HandleGameStart;
        model.OnGameEnded += HandleGameEnd;
    }

    void HandleGameStart ()
    {
        gameController = new GameController(
            model.GameModel,
            GameSessionViewFactory.Create(),
            tileSpriteDatabase,
            view.ContentCamera
        );

        gameController.Initialize();
    }

    void HandleGameEnd (bool _)
    {
        gameController.Dispose();
    }
}
