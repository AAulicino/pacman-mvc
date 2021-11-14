using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class ActorController : IDisposable
{
    readonly IActorModel model;
    readonly ActorView view;

    protected Vector2 targetPosition;
    protected Vector2 originPosition;
    float delta;

    readonly Quaternion up = Quaternion.Euler(new Vector3(0, 0, 90));
    readonly Quaternion right = Quaternion.Euler(new Vector3(0, 0, 0));
    readonly Quaternion left = Quaternion.Euler(new Vector3(0, 0, 0));
    readonly Quaternion down = Quaternion.Euler(new Vector3(0, 0, -90));

    public ActorController (IActorModel model, ActorView view)
    {
        this.model = model;
        this.view = view;

        originPosition = model.Position;
        targetPosition = originPosition;
        view.transform.position = originPosition;

        model.OnPositionChanged += HandlePositionChanged;
        model.OnDirectionChanged += HandleDirectionChanged;
        model.OnEnableChange += HandleEnableChange;
    }

    void HandleEnableChange (bool enabled)
    {
        if (enabled)
            view.StartCoroutine(SmoothReposition());
    }

    void HandlePositionChanged ()
    {
        originPosition = targetPosition;
        targetPosition = model.Position;
        delta = 0;
    }

    IEnumerator SmoothReposition ()
    {
        while (true)
        {
            delta += Time.deltaTime;
            view.transform.position = Vector3.Lerp(
                originPosition,
                targetPosition,
                delta / model.MovementTime
            );
            yield return null;
        }
    }

    void HandleDirectionChanged ()
    {
        view.transform.rotation = model.Direction switch
        {
            Direction.Up => up,
            Direction.Right => right,
            Direction.Left => left,
            Direction.Down => down,
            _ => throw new NotImplementedException()
        };

        if (model.Direction == Direction.Left)
            view.transform.localScale = new Vector3(-1, 1, 1);
        else
            view.transform.localScale = new Vector3(1, 1, 1);
    }

    public virtual void Dispose ()
    {
        model.OnPositionChanged -= HandlePositionChanged;
        model.OnDirectionChanged -= HandleDirectionChanged;
        model.OnEnableChange -= HandleEnableChange;

        Object.Destroy(view.gameObject);
    }
}
