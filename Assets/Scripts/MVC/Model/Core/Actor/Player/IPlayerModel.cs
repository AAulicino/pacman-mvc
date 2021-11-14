using System;

public interface IPlayerModel : IActorModel
{
    event Action OnTeleport;

    void PowerUp ();
}
