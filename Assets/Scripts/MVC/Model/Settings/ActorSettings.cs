using System;
using UnityEngine;

[Serializable]
public class ActorSettings : IActorSettings
{
    [SerializeField] float movementTime;

    public float MovementTime => movementTime;
}
