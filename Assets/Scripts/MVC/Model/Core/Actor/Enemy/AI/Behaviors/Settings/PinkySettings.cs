using System;
using UnityEngine;

[Serializable]
public class PinkySettings : BaseBehaviorSettings, IPinkySettings
{
    [SerializeField] int leadingTilesAheadOfPacman;

    public int LeadingTilesAheadOfPacman => leadingTilesAheadOfPacman;
}
