using System;
using System.Linq;
using UnityEngine;

public class GameModel : IGameModel
{
    public event Action<Vector2Int> OnFoodCollected;

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
        enemyManager.OnEnemyPositionChanged += HandlePositionChanged;
    }

    public void Initialize ()
    {
        Player.Enable();
        enemyManager.Initialize();
    }

    void HandlePlayerPositionChange ()
    {
        if (Enemies != null && Enemies.Any(x => x.Position == Player.Position))
        {
            EndGame();
            return;
        }

        if (CollectiblesManager.TryCollect(Player.Position, out CollectibleType type))
        {
            if (type == CollectibleType.PowerUp)
            {
                Player.PowerUp();
                enemyManager.TriggerFrightenedMode();
            }
            OnFoodCollected?.Invoke(Player.Position);
        }
    }

    void HandlePositionChanged (IEnemyModel enemy)
    {
        if (enemy.Position == Player.Position)
            EndGame();
    }

    void EndGame ()
    {
        Player.Die();
        foreach (IEnemyModel enemy in Enemies)
            enemy.Disable();
    }

    public void Dispose ()
    {
        Player.OnPositionChanged -= HandlePlayerPositionChange;
    }
}
