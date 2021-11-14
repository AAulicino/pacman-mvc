using System;

public interface IGameModel : IDisposable
{
    event Action<bool> OnGameEnded;

    IMapModel Map { get; }
    IPlayerModel Player { get; }
    IEnemyModel[] Enemies { get; }
    ICollectiblesManagerModel CollectiblesManager { get; }

    void Initialize ();
}
