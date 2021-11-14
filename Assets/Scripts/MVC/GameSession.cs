using UnityEngine;

public class GameSession : MonoBehaviour, ICoroutineRunner
{
    [SerializeField] ContentFitterCamera contentCamera;
    [SerializeField] MapView mapView;

    IMapModel mapModel;
    MapController mapController;

    [RuntimeInitializeOnLoadMethod]
    static void InitializeOnLoad ()
    {
        Instantiate(Resources.Load<GameSession>("GameSession"));
    }

    void Awake ()
    {
        Tile[,] map = LoadMap(0);

        IActorSettings actorSettings = JsonUtility.FromJson<ActorSettings>(
            Resources.Load<TextAsset>("Settings/ActorSettings").text
        );

        IGameSettings gameSettings = JsonUtility.FromJson<GameSettings>(
            Resources.Load<TextAsset>("Settings/GameSettings").text
        );

        IEnemiesBehaviorSettings enemiesSettings = JsonUtility.FromJson<EnemiesBehaviorSettings>(
            Resources.Load<TextAsset>("Settings/EnemiesBehaviorSettings").text
        );

        PlayerModel player = new PlayerModel(
            map,
            this,
            actorSettings,
            InputProvider.Instance,
            TimeProvider.Instance
        );

        ICollectiblesManagerModel collectiblesManager = CollectiblesManagerModelFactory.Create(map);

        mapModel = new MapModel(
            map,
            player,
            EnemyManagerFactory.Create(
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

        mapController = new MapController(
            mapModel,
            mapView,
            Resources.Load<MapTileSpriteDatabase>("Databases/MapTileSpriteDatabase")
        );

        mapController.Initialize();
        mapModel.Initialize();

        const float TILE_PIVOT_OFFSET = 0.5f; //center

        contentCamera.SetViewContent(
            new CameraViewContent(
                new Vector3(-TILE_PIVOT_OFFSET, -TILE_PIVOT_OFFSET, 0),
                new Vector3(
                    map.GetLength(0) - TILE_PIVOT_OFFSET,
                    map.GetLength(1) - TILE_PIVOT_OFFSET,
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
