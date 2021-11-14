using UnityEngine;

public class GameSession : MonoBehaviour, ICoroutineRunner
{
    [SerializeField] ContentFitterCamera contentCamera;
    [SerializeField] GameView mapView;

    IGameModel gameModel;
    GameUIController mapUIController;

    [RuntimeInitializeOnLoadMethod]
    static void InitializeOnLoad ()
    {
        Instantiate(Resources.Load<GameSession>("GameSession"));
    }

    void Awake ()
    {
        IActorSettings actorSettings = JsonUtility.FromJson<ActorSettings>(
            Resources.Load<TextAsset>("Settings/ActorSettings").text
        );

        IGameSettings gameSettings = JsonUtility.FromJson<GameSettings>(
            Resources.Load<TextAsset>("Settings/GameSettings").text
        );

        IEnemiesBehaviorSettings enemiesSettings = JsonUtility.FromJson<EnemiesBehaviorSettings>(
            Resources.Load<TextAsset>("Settings/EnemiesBehaviorSettings").text
        );

        Tile[,] tiles = LoadMap(0);
        IMapModel map = new MapModel(tiles);

        PlayerModel player = new PlayerModel(
            map,
            this,
            actorSettings,
            InputProvider.Instance,
            TimeProvider.Instance
        );

        ICollectiblesManagerModel collectiblesManager = CollectiblesManagerModelFactory.Create(map);

        gameModel = new GameModel(
            map,
            player,
            EnemyManagerFactory.Create(
                new PathFinder(tiles),
                map,
                actorSettings,
                gameSettings,
                this,
                player,
                collectiblesManager,
                enemiesSettings
            ),
            collectiblesManager
        );

        GameController gameController = new GameController(
            gameModel,
            mapView,
            Resources.Load<MapTileSpriteDatabase>("Databases/MapTileSpriteDatabase")
        );

        new GameUIController(gameModel, MapUIViewFactory.Create());

        gameController.Initialize();
        gameModel.StartGame();

        const float TILE_PIVOT_OFFSET = 0.5f; //center

        contentCamera.SetViewContent(
            new CameraViewContent(
                new Vector3(-TILE_PIVOT_OFFSET, -TILE_PIVOT_OFFSET, 0),
                new Vector3(
                    map.Width - TILE_PIVOT_OFFSET,
                    map.Height - TILE_PIVOT_OFFSET,
                    0
                )
            )
        );
    }

    Tile[,] LoadMap (int mapId)
    {
        string mapString = Resources.Load<TextAsset>("Maps/Map" + mapId).text;
        return MapParser.Parse(mapString);
    }
}
