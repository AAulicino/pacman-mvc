using System;
using UnityEngine;

public interface IGameModel : IDisposable
{
    IMapModel Map { get; }
    IPlayerModel Player { get; }
    IEnemyModel[] Enemies { get; }
    ICollectiblesManagerModel CollectiblesManager { get; }

    event Action<bool> OnGameEnded;

    void StartGame ();
}
