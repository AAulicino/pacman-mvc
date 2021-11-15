using System;
using Object = UnityEngine.Object;

public class GameUIController : IDisposable
{
    readonly IGameModel model;
    readonly GameUIView uiView;

    CollectiblesManagerUIController collectiblesManagerUIController;

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
    }

    public void Dispose ()
    {
        collectiblesManagerUIController.Dispose();
        Object.Destroy(uiView.gameObject);
    }
}
