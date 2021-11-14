
using System;
using UnityEngine;

[Serializable]
public class GameSettings : IGameSettings
{
    [SerializeField] float chaseDuration;
    [SerializeField] float scatterDuration;
    [SerializeField] float frightenedDuration;

    public float ChaseDuration => chaseDuration;
    public float ScatterDuration => scatterDuration;
    public float FrightenedDuration => frightenedDuration;
}
