using System;
using UnityEngine;

public interface IMapModel : IDisposable
{
    event Action<Vector2Int> OnFoodCollected;
    Tile[,] Map { get; }
    IPlayerModel Player { get; }
    IEnemyModel[] Enemies { get; }

    void Initialize ();
}
