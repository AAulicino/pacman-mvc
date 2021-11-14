using UnityEngine;

public static class PlayerViewFactory
{
    public static PlayerView Create ()
    {
        return Object.Instantiate(Resources.Load<PlayerView>("Player/PlayerView"));
    }
}
