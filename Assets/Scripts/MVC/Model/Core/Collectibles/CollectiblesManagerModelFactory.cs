public static class CollectiblesManagerModelFactory
{
    public static ICollectiblesManagerModel Create (IMapModel map)
    {
        return new CollectiblesManagerModel(map, new CollectibleModelFactory());
    }
}
