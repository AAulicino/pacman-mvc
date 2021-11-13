public static class CollectiblesManagerModelFactory
{
    public static ICollectiblesManagerModel Create (Tile[,] map)
    {
        return new CollectiblesManagerModel(map, new CollectibleModelFactory());
    }
}
