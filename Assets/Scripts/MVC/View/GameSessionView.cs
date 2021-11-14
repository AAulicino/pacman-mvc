using UnityEngine;

public class GameSessionView : MonoBehaviour, ICoroutineRunner
{
    [SerializeField] ContentFitterCamera contentCamera;

    [field: SerializeField]
    public GameView GameView { get; private set; }

    public void SetCameraViewContent (CameraViewContent cameraViewContent)
    {
        contentCamera.SetViewContent(cameraViewContent);
    }
}
