using UnityEngine;

public class GameUIView : MonoBehaviour
{
    [field: SerializeField]
    public CollectiblesManagerUIView CollectiblesManagerUIView { get; private set; }

    [field: SerializeField]
    public GameInputUIView[] InputUIViews { get; private set; }
}
