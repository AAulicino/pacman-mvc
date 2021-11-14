using UnityEngine;

public static class MapUIViewFactory
{
    public static MapUIView Create ()
    {
        return Object.Instantiate(Resources.Load<MapUIView>("MapUIView"));
    }
}
