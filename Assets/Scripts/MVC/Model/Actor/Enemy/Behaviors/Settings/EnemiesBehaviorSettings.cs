using System;
using UnityEngine;

[Serializable]
public class EnemiesBehaviorSettings : IEnemiesBehaviorSettings
{
    [SerializeField] BlinkySettings blinky;
    [SerializeField] PinkySettings pinky;
    [SerializeField] InkySettings inky;
    [SerializeField] ClydeSettings clyde;

    public BlinkySettings Blinky => blinky;
    public PinkySettings Pinky => pinky;
    public InkySettings Inky => inky;
    public ClydeSettings Clyde => clyde;
}
