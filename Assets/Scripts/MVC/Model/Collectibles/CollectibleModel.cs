using UnityEngine;

public class CollectibleModel : ICollectibleModel
{
    public CollectibleType Type { get; }
    public Vector2Int Position { get; }

    public CollectibleModel (Vector2Int position, CollectibleType type)
    {
        Position = position;
        Type = type;
    }
}
