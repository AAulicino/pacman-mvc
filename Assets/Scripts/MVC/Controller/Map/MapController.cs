using UnityEngine;

public class GameController
{
    readonly GameView view;
    readonly IGameModel model;
    readonly MapTileSpriteDatabase spriteDatabase;

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
        int width = model.Map.Width;
        int height = model.Map.Height;

        view.Initialize(new Vector2Int(width, height));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                view.SetTileSprite(new Vector2Int(x, y), spriteDatabase.GetSprite(model.Map[x, y]));
            }
        }

        collectibleController = new CollectiblesManagerController(model.CollectiblesManager);
        collectibleController.Initialize();

        playerController = new PlayerController(model.Player, PlayerViewFactory.Create());

        foreach (IEnemyModel enemy in model.Enemies)
            enemyController = new EnemyController(enemy, EnemyViewFactory.Create(enemy.EnemyType));
    }
}
