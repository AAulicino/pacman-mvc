using System;
using System.Linq;
using UnityEngine;

public class MapModel : IMapModel
{
    public event Action<Vector2Int> OnFoodCollected;

    readonly Tile[,] map;
    readonly IEnemyManager enemyManager;
    readonly bool[,] foodMap;

    public ICollectiblesManagerModel CollectiblesManager { get; }

    public IPlayerModel Player { get; }
    public IEnemyModel[] Enemies => enemyManager.Enemies;

    public Tile[,] Map => map;

    public MapModel (
        Tile[,] map,
        IPlayerModel player,
        IEnemyManager enemyManager,
        ICollectiblesManagerModel collectiblesManager
    )
    {
        this.map = map;
        this.Player = player;
        this.enemyManager = enemyManager;
        CollectiblesManager = collectiblesManager;
        foodMap = new bool[map.GetLength(0), map.GetLength(1)];

        player.OnPositionChanged += HandlePlayerPositionChange;
    }

    public void Initialize ()
    {
        FillMapWithFood();
        Player.Enable();
        enemyManager.Initialize();
    }

    void HandlePlayerPositionChange ()
    {
        if (Enemies != null && Enemies.Any(x => x.Position == Player.Position))
        {
            Player.Die();
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

    void FillMapWithFood ()
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
                if (map[x, y] == Tile.Path)
                    foodMap[x, y] = true;
        }
    }

    public void Dispose ()
    {
        Player.OnPositionChanged -= HandlePlayerPositionChange;
    }
}
