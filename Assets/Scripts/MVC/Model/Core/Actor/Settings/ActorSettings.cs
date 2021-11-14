using System;
using UnityEngine;

[Serializable]
public class ActorSettings : IActorSettings
{
    [SerializeField] float movementTime;
    [SerializeField] float frightenedMoveTime;

    public float MovementTime => movementTime;
    public float FrightenedMoveTime => frightenedMoveTime;
}
