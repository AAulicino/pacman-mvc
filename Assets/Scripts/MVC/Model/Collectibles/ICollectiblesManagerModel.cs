using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectiblesManagerModel
{
    event Action<ICollectibleModel> OnCollect;

    ICollection<ICollectibleModel> Collectibles { get; }
    int TotalCollectibles { get; }
    int CollectedCount { get; }

    bool TryCollect (Vector2Int position, out CollectibleType type);
}
