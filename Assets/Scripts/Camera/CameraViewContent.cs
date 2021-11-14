using UnityEngine;

public class CameraViewContent : ICameraViewContent
{
    public Vector3 Min { get; }
    public Vector3 Max { get; }
    public Bounds Bounds { get; }

    public CameraViewContent (Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;

        Vector3 bottomLeft = min;
        Vector3 topRight = max;
        GetCorners(ref bottomLeft, ref topRight);
        Bounds bounds = new Bounds();
        bounds.SetMinMax(bottomLeft, topRight);
        Bounds = bounds;
    }

    void GetCorners (ref Vector3 bottomLeft, ref Vector3 topRight)
    {
        if (topRight.x < bottomLeft.x)
        {
            float temp = topRight.x;
            topRight.x = bottomLeft.x;
            bottomLeft.x = temp;
        }

        if (topRight.y < bottomLeft.y)
        {
            float temp = topRight.y;
            topRight.y = bottomLeft.y;
            bottomLeft.y = temp;
        }
    }
}
