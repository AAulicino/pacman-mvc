using System;
using UnityEngine;

[Serializable]
public class InkySettings : BaseBehaviorSettings, IInkySettings
{
    [SerializeField] int leadingTilesAheadOfPacman;
    [SerializeField] int collectedRequirement;

    public int CollectedRequirement => collectedRequirement;
    public int LeadingTilesAheadOfPacman => leadingTilesAheadOfPacman;
}
