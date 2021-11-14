using UnityEngine;

public class CollectibleModelFactory : ICollectibleModelFactory
{
    public ICollectibleModel Create (Vector2Int position, CollectibleType type)
    {
        return new CollectibleModel(position, type);
    }
}
