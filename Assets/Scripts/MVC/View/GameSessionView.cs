using UnityEngine;

public class GameSessionView : MonoBehaviour, ICoroutineRunner
{
    [field: SerializeField]
    public ContentFitterCamera ContentCamera { get; private set; }
}
