using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameController : IDisposable
{
    readonly GameView view;
    readonly IGameModel model;
    readonly MapTileSpriteDatabase spriteDatabase;
    readonly ContentFitterCamera contentFitter;

    MapController mapController;
    CollectiblesManagerController collectibleController;
    PlayerController playerController;
    EnemyController[] enemyControllers;

    public GameController (
        IGameModel model,
        GameView view,
        MapTileSpriteDatabase spriteDatabase,
        ContentFitterCamera contentFitter
    )
    {
        this.model = model;
        this.view = view;
        this.spriteDatabase = spriteDatabase;
        this.contentFitter = contentFitter;
    }

    public void Initialize ()
    {
        mapController = new MapController(model.Map, view.Map, spriteDatabase, contentFitter);
        mapController.Initialize();

        collectibleController = new CollectiblesManagerController(model.CollectiblesManager);
        collectibleController.Initialize(view.transform);

        playerController = new PlayerController(model.Player, PlayerViewFactory.Create());

        enemyControllers = new EnemyController[model.Enemies.Length];
        for (int i = 0; i < model.Enemies.Length; i++)
        {
            IEnemyModel enemy = model.Enemies[i];
            enemyControllers[i] = new EnemyController(enemy, EnemyViewFactory.Create(enemy.EnemyType));
        }
    }

    public void Dispose ()
    {
        foreach (EnemyController controller in enemyControllers)
            controller.Dispose();
        playerController.Dispose();
        collectibleController.Dispose();
        mapController.Dispose();

        Object.Destroy(view.gameObject);
    }
}
