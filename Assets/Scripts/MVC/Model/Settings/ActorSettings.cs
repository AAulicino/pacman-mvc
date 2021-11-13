using System;
using UnityEngine;

[Serializable]
public class ActorSettings : IActorSettings
{
    [field: SerializeField]
    public float MovementTime { get; private set; }
}
