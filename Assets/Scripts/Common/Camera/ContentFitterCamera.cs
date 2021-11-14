using UnityEngine;

public class ContentFitterCamera : MonoBehaviour
{
    [SerializeField] new Camera camera;

    public void SetViewContent (ICameraViewContent content)
    {
        float screenRatio = Screen.safeArea.width / (float)Screen.safeArea.height;
        float targetHeight = content.Bounds.size.y;
        float targetRatio = content.Bounds.size.x / targetHeight;

        float orthoSize = targetHeight / 2f;

        Vector3 pos = content.Bounds.center;

        if (screenRatio >= targetRatio)
            camera.orthographicSize = orthoSize;
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            camera.orthographicSize = orthoSize * differenceInSize;
        }

        camera.transform.localPosition = Vector3.zero;
        transform.position = pos;
    }
}
