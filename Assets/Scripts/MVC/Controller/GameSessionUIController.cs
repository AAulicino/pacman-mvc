public class GameSessionUIController
{
    readonly GameSessionModel model;
    readonly GameSessionUIView uiView;

    public GameSessionUIController (GameSessionModel model, GameSessionUIView uiView)
    {
        this.model = model;

        model.OnGameEnded += HandleGameEnded;
        uiView.OnClick += HandleUIViewClick;
    }

    public void Initialize ()
    {
        uiView.SetGameEndMessageActive(false);
        uiView.gameObject.SetActive(true);
    }

    void HandleGameEnded (bool victory)
    {
        uiView.SertEndGameMessageText(victory ? "Victory" : "Defeat");
        uiView.SetGameEndMessageActive(true);
        uiView.SetStartGameUIActive(true);
    }

    void HandleUIViewClick ()
    {
        uiView.gameObject.SetActive(false);
        model.StartNewGame();
    }
}
