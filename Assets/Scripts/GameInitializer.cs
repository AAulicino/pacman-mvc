using UnityEngine;

public static class GameInitializer
{
    static IActorSettings actorSettings;
    static IGameSettings gameSettings;
    static IEnemiesBehaviorSettings enemiesSettings;

    static GameSessionModel model;
    static GameSessionController controller;
    static GameSessionUIController uiController;

    [RuntimeInitializeOnLoadMethod]
    static void InitializeOnLoad ()
    {
        LoadSettings();

        GameSessionView view = Object.Instantiate(Resources.Load<GameSessionView>("GameSessionView"));

        model = new GameSessionModel(
            new GameModelFactory(view, actorSettings, gameSettings, enemiesSettings),
            new MapFactory()
        );

        controller = new GameSessionController(
            model,
            view,
            Resources.Load<MapTileSpriteDatabase>("Databases/MapTileSpriteDatabase")
        );

        uiController = new GameSessionUIController(
            model,
            view,
            Object.Instantiate(Resources.Load<GameSessionUIView>("GameSessionUIView"))
        );

        uiController.Initialize();
    }

    static void LoadSettings ()
    {
        actorSettings = ReadSettings<ActorSettings>("ActorSettings");
        gameSettings = ReadSettings<GameSettings>("GameSettings");
        enemiesSettings = ReadSettings<EnemiesBehaviorSettings>("EnemiesBehaviorSettings");
    }

    static T ReadSettings<T> (string path)
        => JsonUtility.FromJson<T>(Resources.Load<TextAsset>("Settings/" + path).text);
}
