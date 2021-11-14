public class GameUIController
{
    readonly IGameModel model;
    readonly GameUIView uiView;

    public GameUIController (IGameModel model, GameUIView uiView)
    {
        this.model = model;

        model.OnGameEnded += HandleGameEnded;
        uiView.OnClick += HandleUIViewClick;

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
        model.StartGame();
    }
}
