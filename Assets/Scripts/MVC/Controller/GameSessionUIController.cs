public class GameSessionUIController
{
    readonly GameSessionModel model;
    readonly GameSessionView view;
    readonly GameSessionUIView uiView;

    GameUIController gameUIController;

    public GameSessionUIController (
        GameSessionModel model,
        GameSessionView view,
        GameSessionUIView uiView
    )
    {
        this.model = model;
        this.view = view;
        this.uiView = uiView;

        model.OnGameStart += HandleGameStart;
        model.OnGameEnded += HandleGameEnded;
        uiView.OnClick += HandleUIViewClick;
    }

    public void Initialize ()
    {
        uiView.Canvas.worldCamera = view.ContentCamera.Camera;
        uiView.SetGameEndMessageActive(false);
        uiView.gameObject.SetActive(true);
    }

    void HandleGameStart ()
    {
        gameUIController = new GameUIController(
            model.GameModel,
            GameSessionViewFactory.CreateUI(uiView.transform)
        );
        gameUIController.Initialize();
        uiView.gameObject.SetActive(false);
    }

    void HandleGameEnded (bool victory)
    {
        gameUIController.Dispose();

        uiView.SertEndGameMessageText(victory ? "Victory" : "Defeat");
        uiView.SetGameEndMessageActive(true);
        uiView.SetStartGameButtonText("Play Again");
        uiView.SetStartGameUIActive(true);
        uiView.gameObject.SetActive(true);
    }

    void HandleUIViewClick ()
    {
        model.StartNewGame();
    }
}
