using System;
using UnityEngine;

public interface IEnemyAI : IDisposable
{
    EnemyType EnemyType { get; }
    Vector2Int Position { get; }
    Direction Direction { get; }

    void Initialize ();
    void Advance ();
}
