public class GameModelFactory : IGameModelFactory
{
    readonly ICoroutineRunner coroutineRunner;
    readonly IActorSettings actorSettings;
    readonly IGameSettings gameSettings;
    readonly IEnemiesBehaviorSettings enemiesSettings;

    public GameModelFactory (
        ICoroutineRunner coroutineRunner,
        IActorSettings actorSettings,
        IGameSettings gameSettings,
        IEnemiesBehaviorSettings enemiesSettings
    )
    {
        this.coroutineRunner = coroutineRunner;
        this.actorSettings = actorSettings;
        this.gameSettings = gameSettings;
        this.enemiesSettings = enemiesSettings;
    }

    public IGameModel Create (Tile[,] tiles)
    {
        IMapModel map = new MapModel(tiles);

        PlayerModel player = new PlayerModel(
            map,
            coroutineRunner,
            actorSettings,
            gameSettings,
            InputProvider.Instance,
            TimeProvider.Instance
        );

        ICollectiblesManagerModel collectiblesManager = CollectiblesManagerModelFactory.Create(map);
        collectiblesManager.Initialize();

        return new GameModel(
            map,
            player,
            EnemyManagerFactory.Create(
                new PathFinder(tiles),
                map,
                actorSettings,
                gameSettings,
                coroutineRunner,
                player,
                collectiblesManager,
                enemiesSettings
            ),
            collectiblesManager
        );
    }
}
