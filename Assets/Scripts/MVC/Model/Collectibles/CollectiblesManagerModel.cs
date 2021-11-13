using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManagerModel : ICollectiblesManagerModel
{
    public event Action<ICollectibleModel> OnCollect;

    public ICollection<ICollectibleModel> Collectibles => collectibles.Values;

    readonly Dictionary<Vector2Int, ICollectibleModel> collectibles =
        new Dictionary<Vector2Int, ICollectibleModel>();

    public CollectiblesManagerModel (Tile[,] map, ICollectibleModelFactory collectibleFactory)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
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
    }

    public bool TryCollect (Vector2Int position, out CollectibleType type)
    {
        if (collectibles.TryGetValue(position, out ICollectibleModel collectible))
        {
            collectibles.Remove(position);
            type = collectible.Type;
            OnCollect?.Invoke(collectible);
            return true;
        }
        type = default;
        return false;
    }
}
