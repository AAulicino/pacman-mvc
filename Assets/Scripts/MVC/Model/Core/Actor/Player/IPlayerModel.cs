using System;

public interface IPlayerModel : IActorModel
{
    event Action OnTeleport;

    bool HasPowerUp { get; }

    void PowerUp ();
}
