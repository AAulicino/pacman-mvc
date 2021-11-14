using UnityEngine;

public class PlayerController : ActorController
{
    readonly IPlayerModel model;
    readonly PlayerView view;

    public PlayerController (IPlayerModel model, PlayerView view) : base(model, view)
    {
        this.model = model;
        this.view = view;

        model.OnTeleport += HandleTeleport;
    }

    void HandleTeleport ()
    {
        originPosition = model.Position;
        targetPosition = model.Position;
        view.transform.position = (Vector2)model.Position;
    }
}
