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

    public ActorController (IActorModel model, ActorView view)
    {
        this.model = model;
        this.view = view;

        originPosition = model.Position;
        targetPosition = originPosition;
        view.transform.position = originPosition;

        model.OnPositionChanged += HandlePositionChanged;
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

    public virtual void Dispose ()
    {
        model.OnPositionChanged -= HandlePositionChanged;
        model.OnEnableChange -= HandleEnableChange;

        Object.Destroy(view.gameObject);
    }
}
