using System;
using UnityEngine;

public interface IEnemyAI
{
    event Action OnActiveModeChanged;

    EnemyType EnemyType { get; }
    Vector2Int Position { get; }
    Direction Direction { get; }
    EnemyAIMode ActiveMode { get; }

    void Initialize ();
    void Advance ();
    void Die ();
}
