using UnityEngine;

public interface ICollectibleModelFactory
{
    ICollectibleModel Create (Vector2Int position, CollectibleType type);
}
