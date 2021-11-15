using System;
using Object = UnityEngine.Object;

public class CollectiblesManagerUIController : IDisposable
{
    private readonly ICollectiblesManagerModel model;
    private readonly CollectiblesManagerUIView uiView;

    public CollectiblesManagerUIController (
        ICollectiblesManagerModel model,
        CollectiblesManagerUIView uiVIew
    )
    {
        this.model = model;
        this.uiView = uiVIew;

        model.OnCollect += HandleOnCollect;
    }

    void HandleOnCollect (ICollectibleModel collectible)
    {
        uiView.SetCollectedCountText(model.CollectedCount + " " + model.TotalCollectibles);
    }

    public void Dispose ()
    {
        model.OnCollect -= HandleOnCollect;
        Object.Destroy(uiView.gameObject);
    }
}
