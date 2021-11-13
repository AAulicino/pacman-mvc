using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManagerController
{
    readonly ICollectiblesManagerModel model;
    readonly Dictionary<Vector2Int, CollectibleController> collectibles;

    public CollectiblesManagerController (ICollectiblesManagerModel model)
    {
        this.model = model;
        collectibles = new Dictionary<Vector2Int, CollectibleController>(model.Collectibles.Count);
        model.OnCollect += HandleCollectibleCollected;
    }

    public void Initialize ()
    {
        foreach (ICollectibleModel model in model.Collectibles)
        {
            collectibles.Add(
                model.Position,
                new CollectibleController(model, CollectibleViewFactory.Create(model.Type)
            ));
        }
    }

    void HandleCollectibleCollected (ICollectibleModel model)
    {
        CollectibleController controller = collectibles[model.Position];
        controller.Destroy();
        collectibles.Remove(model.Position);
    }
}
