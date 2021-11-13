using UnityEngine;

public interface ICollectibleModel
{
    CollectibleType Type { get; }
    Vector2Int Position { get; }
}
