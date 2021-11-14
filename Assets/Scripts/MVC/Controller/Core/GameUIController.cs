using System;
using Object = UnityEngine.Object;

public class GameUIController : IDisposable
{
    readonly IGameModel model;
    readonly GameUIView uiView;

    public GameUIController (IGameModel model, GameUIView uiView)
    {
        this.model = model;
        this.uiView = uiView;
    }

    public void Dispose ()
    {
        Object.Destroy(uiView.gameObject);
    }
}
