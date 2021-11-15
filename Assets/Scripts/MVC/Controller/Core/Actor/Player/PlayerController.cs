using System;
using UnityEngine;

public class PlayerController : ActorController
{
    readonly IPlayerModel model;
    readonly PlayerView view;

    readonly Quaternion up = Quaternion.Euler(new Vector3(0, 0, 90));
    readonly Quaternion side = Quaternion.Euler(new Vector3(0, 0, 0));
    readonly Quaternion down = Quaternion.Euler(new Vector3(0, 0, -90));

    public PlayerController (IPlayerModel model, PlayerView view) : base(model, view)
    {
        this.model = model;
        this.view = view;

        model.OnTeleport += HandleTeleport;
        model.OnDirectionChanged += HandleDirectionChanged;
    }

    void HandleTeleport ()
    {
        originPosition = model.Position;
        targetPosition = model.Position;
        view.transform.position = (Vector2)model.Position;
    }

    void HandleDirectionChanged ()
    {
        view.transform.rotation = model.Direction switch
        {
            Direction.Up => up,
            Direction.Right => side,
            Direction.Left => side,
            Direction.Down => down,
            _ => throw new NotImplementedException()
        };

        if (model.Direction == Direction.Left)
            view.transform.localScale = new Vector3(-1, 1, 1);
        else
            view.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void Dispose ()
    {
        model.OnDirectionChanged -= HandleDirectionChanged;
        model.OnTeleport -= HandleTeleport;
        base.Dispose();
    }
}
