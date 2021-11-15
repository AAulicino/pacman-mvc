using System;
using Object = UnityEngine.Object;

public class GameUIController : IDisposable
{
    readonly IGameModel model;
    readonly GameUIView uiView;

    CollectiblesManagerUIController collectiblesManagerUIController;
    GameInputUIController inputUIController;

    public GameUIController (IGameModel model, GameUIView uiView)
    {
        this.model = model;
        this.uiView = uiView;
    }

    public void Initialize ()
    {
        collectiblesManagerUIController = new CollectiblesManagerUIController(
            model.CollectiblesManager,
            uiView.CollectiblesManagerUIView
        );

        inputUIController = new GameInputUIController(
            model.Input,
            uiView.InputUIViews
        );
    }

    public void Dispose ()
    {
        inputUIController.Dispose();
        collectiblesManagerUIController.Dispose();
        Object.Destroy(uiView.gameObject);
    }
}
