using UnityEngine;

public static class GameSessionViewFactory
{
    public static GameView Create ()
        => Object.Instantiate(Resources.Load<GameView>("Game/GameView"));

    public static GameUIView CreateUI (Transform parent)
        => Object.Instantiate(Resources.Load<GameUIView>("Game/GameUIView"), parent);
}
