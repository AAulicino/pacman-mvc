using System;
using UnityEngine;

public interface IActorModel
{
    event Action OnPositionChanged;
    event Action OnDirectionChanged;
    event Action<bool> OnEnableChange;

    Vector2Int Position { get; }
    Direction Direction { get; }
    float MovementTime { get; }

    void Enable ();
    void Die ();
}
