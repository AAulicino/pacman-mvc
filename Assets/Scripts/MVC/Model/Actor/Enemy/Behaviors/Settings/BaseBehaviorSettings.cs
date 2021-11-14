using System;
using UnityEngine;

[Serializable]
public abstract class BaseBehaviorSettings : IBaseBehaviorSettings
{
    [SerializeField] ScatterPosition scatterPosition;

    public ScatterPosition ScatterPosition => scatterPosition;
}
