using System;
using UnityEngine;

public interface IEnemyAI : IDisposable
{
    EnemyType EnemyType { get; }
    Vector2Int Position { get; }
    Direction Direction { get; }
    EnemyAIMode ActiveMode { get; }

    event Action OnActiveModeChanged;

    void Initialize ();
    void Advance ();
}
