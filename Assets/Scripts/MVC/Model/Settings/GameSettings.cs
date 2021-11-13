
using System;
using UnityEngine;

[Serializable]
public class GameSettings : IGameSettings
{
    [field: SerializeField]
    public float ChaseDuration { get; private set; }
    [field: SerializeField]
    public float ScatterDuration { get; private set; }
    [field: SerializeField]
    public float FrightenedDuration { get; private set; }
}
