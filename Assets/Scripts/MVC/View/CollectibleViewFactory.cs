using UnityEngine;
using Object = UnityEngine.Object;

public static class CollectibleViewFactory
{
    public static CollectibleView Create (CollectibleType enemyType)
    {
        switch (enemyType)
        {
            case CollectibleType.Default:
                return Object.Instantiate(Resources.Load<CollectibleView>("DefaultCollectible"));
            case CollectibleType.PowerUp:
                return Object.Instantiate(Resources.Load<CollectibleView>("PowerUpCollectible"));

            default:
                throw new System.NotImplementedException();
        }
    }
}
