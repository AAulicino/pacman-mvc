using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class CollectibleController : IDisposable
{
    readonly ICollectibleModel model;
    readonly CollectibleView view;

    public CollectibleController (ICollectibleModel model, CollectibleView view)
    {
        this.model = model;
        this.view = view;

        view.transform.position = (Vector2)model.Position;
        view.name = model.Type + " " + model.Position.ToString();
    }

    public void Destroy ()
    {
        Object.Destroy(view.gameObject);
    }

    public void Dispose ()
    {
        Destroy();
    }
}
