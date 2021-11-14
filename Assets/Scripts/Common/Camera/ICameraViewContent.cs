using UnityEngine;

public interface ICameraViewContent
{
    Vector3 Min { get; }
    Vector3 Max { get; }
    Bounds Bounds { get; }
}
