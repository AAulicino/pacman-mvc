using System;
using System.Linq;

public class GameModel : IGameModel
{
    public event Action<bool> OnGameEnded;

    readonly IEnemyManager enemyManager;

    public IMapModel Map { get; }
    public ICollectiblesManagerModel CollectiblesManager { get; }

    public IPlayerModel Player { get; }
    public IEnemyModel[] Enemies => enemyManager.Enemies;

    public GameModel (
        IMapModel map,
        IPlayerModel player,
        IEnemyManager enemyManager,
        ICollectiblesManagerModel collectiblesManager
    )
    {
        this.Map = map;
        this.Player = player;
        this.enemyManager = enemyManager;
        CollectiblesManager = collectiblesManager;

        player.OnPositionChanged += HandlePlayerPositionChange;
        enemyManager.OnEnemyPositionChanged += HandleEnemyPositionChanged;
        CollectiblesManager.OnAllCollectiblesCollected += HandleAllCollectiblesCollected;
    }

    public void StartGame ()
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
        if (Enemies != null && Enemies.Any(x => x.Position == Player.Position))
        {
            KillPlayer();
            return;
        }
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
            KillPlayer();
    }

    void KillPlayer ()
    {
        Player.Die();
        DisableEnemies();
        OnGameEnded?.Invoke(false);
    }

    void HandleAllCollectiblesCollected ()
    {
        Player.Disable();
        DisableEnemies();
        OnGameEnded?.Invoke(true);
    }

    void DisableEnemies ()
    {
        foreach (IEnemyModel enemy in Enemies)
            enemy.Disable();
    }

    public void Dispose ()
    {
        Player.OnPositionChanged -= HandlePlayerPositionChange;
        Player.Dispose();
        Enemies.ForEach(x => x.Dispose());
        Map.Dispose();
    }
}
