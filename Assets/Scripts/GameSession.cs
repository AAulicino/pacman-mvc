using UnityEngine;

public class GameSession : MonoBehaviour, ICoroutineRunner
{
    IMapModel mapModel;
    MapController mapController;
    MapView mapView;

    [RuntimeInitializeOnLoadMethod]
    static void InitializeOnLoad ()
    {
        new GameObject("GameSession", typeof(GameSession));
    }

    void Awake ()
    {
        mapView = Instantiate(Resources.Load<MapView>("MapView"));

        Tile[,] map = LoadMap(0);

        IActorSettings actorSettings = JsonUtility.FromJson<ActorSettings>(
            Resources.Load<TextAsset>("ActorSettings").text
        );

        IGameSettings gameSettings = JsonUtility.FromJson<GameSettings>(
            Resources.Load<TextAsset>("GameSettings").text
        );

        IEnemiesBehaviorSettings enemiesSettings = JsonUtility.FromJson<EnemiesBehaviorSettings>(
            Resources.Load<TextAsset>("EnemiesBehaviorSettings").text
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
            Resources.Load<MapTileSpriteDatabase>("MapTileSpriteDatabase")
        );

        mapController.Initialize();
        mapModel.Initialize();
    }

    Tile[,] LoadMap (int mapId)
    {
        string mapString = Resources.Load<TextAsset>("Map" + mapId).text;
        return MapParser.Parse(mapString);
    }
}
