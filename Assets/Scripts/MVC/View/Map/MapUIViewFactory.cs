using UnityEngine;

public static class GameUIViewFactory
{
    public static GameUIView Create ()
    {
        return Object.Instantiate(Resources.Load<GameUIView>("GameUIView"));
    }
}
