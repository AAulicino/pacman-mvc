using System;
using System.Linq;

public class GameModel : IGameModel
{
    public event Action<bool> OnGameEnded;

    readonly IEnemyManager enemyManager;

    public IMapModel Map { get; }
    public ICollectiblesManagerModel CollectiblesManager { get; }
    public IGameInputModel Input { get; }

    public IPlayerModel Player { get; }
    public IEnemyModel[] Enemies => enemyManager.Enemies;

    public GameModel (
        IMapModel map,
        IGameInputModel input,
        IPlayerModel player,
        IEnemyManager enemyManager,
        ICollectiblesManagerModel collectiblesManager
    )
    {
        this.Map = map;
        this.Input = input;
        this.Player = player;
        this.enemyManager = enemyManager;
        CollectiblesManager = collectiblesManager;

        player.OnPositionChanged += HandlePlayerPositionChange;
        enemyManager.OnEnemyPositionChanged += HandleEnemyPositionChanged;
        CollectiblesManager.OnAllCollectiblesCollected += HandleAllCollectiblesCollected;
    }

    public void Initialize ()
    {
        Player.Enable();
        enemyManager.Initialize();
    }

    void HandlePlayerPositionChange ()
    {
        EvaluatePlayerCollision();
        EvaluateCollectibles();
    }

    void EvaluatePlayerCollision ()
    {
        IEnemyModel enemy = Enemies.FirstOrDefault(x => x.Position == Player.Position);

        if (enemy == null)
            return;

        HandlePlayerCollisionWithEnemy(enemy);
    }

    void EvaluateCollectibles ()
    {
        if (CollectiblesManager.TryCollect(Player.Position, out CollectibleType type))
        {
            if (type == CollectibleType.PowerUp)
            {
                Player.PowerUp();
                enemyManager.TriggerFrightenedMode();
            }
        }
    }

    void HandleEnemyPositionChanged (IEnemyModel enemy)
    {
        if (enemy.Position == Player.Position)
            HandlePlayerCollisionWithEnemy(enemy);
    }

    void HandlePlayerCollisionWithEnemy (IEnemyModel enemy)
    {
        if (Player.HasPowerUp)
            enemy.Die();
        else
            KillPlayer();
    }

    void KillPlayer ()
    {
        Player.Die();
        enemyManager.Disable();
        OnGameEnded?.Invoke(false);
    }

    void HandleAllCollectiblesCollected ()
    {
        Player.Disable();
        enemyManager.Disable();
        OnGameEnded?.Invoke(true);
    }

    public void Dispose ()
    {
        Player.OnPositionChanged -= HandlePlayerPositionChange;
        Player.Dispose();
        enemyManager.Dispose();
    }
}
