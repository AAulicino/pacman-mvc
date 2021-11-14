using UnityEngine;
using Object = UnityEngine.Object;

public static class CollectibleViewFactory
{
    public static CollectibleView Create (CollectibleType type, Transform parent)
    {
        return Object.Instantiate(
            Resources.Load<CollectibleView>($"Collectibles/{type}Collectible"),
            parent
        );
    }
}
