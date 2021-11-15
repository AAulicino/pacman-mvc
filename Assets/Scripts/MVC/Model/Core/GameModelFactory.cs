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

        GameInputModel input = new GameInputModel(InputProvider.Instance);

        PlayerModel player = new PlayerModel(
            map,
            coroutineRunner,
            actorSettings,
            gameSettings,
            input,
            TimeProvider.Instance
        );

        ICollectiblesManagerModel collectiblesManager = new CollectiblesManagerModel(
            map,
            new CollectibleModelFactory()
        );

        collectiblesManager.Initialize();

        return new GameModel(
            map,
            input,
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
