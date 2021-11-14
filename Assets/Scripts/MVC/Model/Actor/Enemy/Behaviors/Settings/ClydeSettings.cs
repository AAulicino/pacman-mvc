using UnityEngine;

public class ClydeSettings : BaseBehaviorSettings, IClydeSettings
{
    [SerializeField] float collectedRequirementRatio;
    [SerializeField] int disableChaseDistance;

    public float CollectedRequirementRatio => collectedRequirementRatio;
    public int DisableChaseDistance => disableChaseDistance;
}
