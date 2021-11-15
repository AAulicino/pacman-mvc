using System;

public interface IEnemyModel : IActorModel
{
    event Action OnActiveModeChanged;

    EnemyType EnemyType { get; }
    EnemyAIMode ActiveMode { get; }
}
