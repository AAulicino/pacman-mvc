using System;

public class GameController : IDisposable
{
    readonly GameView view;
    readonly IGameModel model;
    readonly MapTileSpriteDatabase spriteDatabase;

    MapController mapController;
    CollectiblesManagerController collectibleController;
    PlayerController playerController;
    EnemyController enemyController;

    public GameController (IGameModel model, GameView view, MapTileSpriteDatabase spriteDatabase)
    {
        this.model = model;
        this.view = view;
        this.spriteDatabase = spriteDatabase;
    }

    public void Initialize ()
    {
        mapController = new MapController(model.Map, view.Map, spriteDatabase);

        collectibleController = new CollectiblesManagerController(model.CollectiblesManager);
        collectibleController.Initialize();

        playerController = new PlayerController(model.Player, PlayerViewFactory.Create());

        foreach (IEnemyModel enemy in model.Enemies)
            enemyController = new EnemyController(enemy, EnemyViewFactory.Create(enemy.EnemyType));
    }

    public void Dispose ()
    {
        enemyController.Dispose();
        playerController.Dispose();
        collectibleController.Dispose();
        mapController.Dispose();
    }
}
