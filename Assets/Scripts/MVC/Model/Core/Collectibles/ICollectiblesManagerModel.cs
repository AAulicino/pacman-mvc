using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectiblesManagerModel
{
    event Action OnAllCollectiblesCollected;
    event Action<ICollectibleModel> OnCollect;

    ICollection<ICollectibleModel> Collectibles { get; }
    int TotalCollectibles { get; }
    int CollectedCount { get; }

    void Initialize ();
    bool TryCollect (Vector2Int position, out CollectibleType type);
}
