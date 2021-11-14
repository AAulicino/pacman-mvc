using UnityEngine;

public class MapFactory : IMapFactory
{
    public Tile[,] LoadMap (int mapId)
        => MapParser.Parse(Resources.Load<TextAsset>("Maps/Map" + mapId).text);
}
