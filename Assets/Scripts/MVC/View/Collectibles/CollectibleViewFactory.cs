using UnityEngine;
using Object = UnityEngine.Object;

public static class CollectibleViewFactory
{
    public static CollectibleView Create (CollectibleType type)
    {
        return Object.Instantiate(Resources.Load<CollectibleView>($"Collectibles/{type}Collectible"));
    }
}
