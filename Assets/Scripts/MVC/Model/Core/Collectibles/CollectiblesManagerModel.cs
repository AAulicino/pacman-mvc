using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManagerModel : ICollectiblesManagerModel
{
    public event Action OnAllCollectiblesCollected;
    public event Action<ICollectibleModel> OnCollect;

    public int TotalCollectibles { get; private set; }
    public int CollectedCount { get; private set; }

    public ICollection<ICollectibleModel> Collectibles => collectibles.Values;

    readonly Dictionary<Vector2Int, ICollectibleModel> collectibles =
        new Dictionary<Vector2Int, ICollectibleModel>();

    readonly ICollectibleModelFactory collectibleFactory;
    readonly IMapModel map;

    public CollectiblesManagerModel (IMapModel map, ICollectibleModelFactory collectibleFactory)
    {
        this.map = map;
        this.collectibleFactory = collectibleFactory;
    }

    public void Initialize ()
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                Tile tile = map[x, y];
                if (tile == Tile.Path)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    collectibles.Add(pos, collectibleFactory.Create(pos, CollectibleType.Default));
                }
                else if (tile == Tile.PowerUp)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    collectibles.Add(pos, collectibleFactory.Create(pos, CollectibleType.PowerUp));
                }
            }
        }
        TotalCollectibles = collectibles.Count;
    }

    public bool TryCollect (Vector2Int position, out CollectibleType type)
    {
        if (collectibles.TryGetValue(position, out ICollectibleModel collectible))
        {
            type = collectible.Type;
            HandleCollectibleCollected(collectible);
            return true;
        }
        type = default;
        return false;
    }

    void HandleCollectibleCollected (ICollectibleModel collectible)
    {
        collectibles.Remove(collectible.Position);
        CollectedCount++;
        OnCollect?.Invoke(collectible);

        if (CollectedCount == TotalCollectibles)
            OnAllCollectiblesCollected?.Invoke();
    }
}
