using System;
using UnityEngine;

public interface IActorModel : IDisposable
{
    event Action OnPositionChanged;
    event Action OnDirectionChanged;
    event Action<bool> OnEnableChange;

    Vector2Int Position { get; }
    Vector2Int DirectionVector { get; }

    Direction Direction { get; }
    float MovementTime { get; }

    void Enable ();
    void Disable ();
    void Die ();
}
