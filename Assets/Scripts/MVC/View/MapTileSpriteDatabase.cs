using System;
using System.Collections.Generic;
using UnityEngine;

public class MapTileSpriteDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] MapTileSpriteDatabaseEntry[] entries;

    Dictionary<Tile, Sprite> sprites;

    public Sprite GetSprite (Tile tile) => sprites[tile];

    void ISerializationCallbackReceiver.OnAfterDeserialize ()
    {
        sprites = new Dictionary<Tile, Sprite>(entries.Length);
        foreach (MapTileSpriteDatabaseEntry entry in entries)
            sprites.Add(entry.Tile, entry.Sprite);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize ()
    {
    }

    [Serializable]
    public class MapTileSpriteDatabaseEntry
    {
        [field: SerializeField]
        public Tile Tile { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }
    }
}
