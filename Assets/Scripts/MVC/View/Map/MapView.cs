using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] SpriteRenderer tilePrefab;

    Dictionary<Vector2Int, SpriteRenderer> tiles;

    public void Initialize (Vector2Int dimension)
    {
        tiles = new Dictionary<Vector2Int, SpriteRenderer>(dimension.x * dimension.y);

        for (int x = 0; x < dimension.x; x++)
        {
            for (int y = 0; y < dimension.y; y++)
            {
                SpriteRenderer tile =
                    Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                tile.name = $"Tile {x},{y}";
                tiles.Add(new Vector2Int(x, y), tile);
            }
        }
    }

    public void SetTileSprite (Vector2Int position, Sprite sprite)
        => tiles[position].sprite = sprite;
}
