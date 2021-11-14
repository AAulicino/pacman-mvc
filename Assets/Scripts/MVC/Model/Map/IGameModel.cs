using System;
using UnityEngine;

public interface IGameModel : IDisposable
{
    event Action<Vector2Int> OnFoodCollected;
    IMapModel Map { get; }
    IPlayerModel Player { get; }
    IEnemyModel[] Enemies { get; }
    ICollectiblesManagerModel CollectiblesManager { get; }

    void Initialize ();
}
