public class GameSessionController
{
    readonly IGameSessionModel model;
    readonly GameSessionView view;
    readonly MapTileSpriteDatabase tileSpriteDatabase;

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
    }

    void HandleGameStart ()
    {
        GameController gameController = new GameController(
            model.GameModel,
            view.GameView,
            tileSpriteDatabase
        );

        gameController.Initialize();
    }
}
