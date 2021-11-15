using UnityEngine;

public class EnemyController : ActorController
{
    readonly IEnemyModel model;
    readonly EnemyView view;

    public EnemyController (IEnemyModel model, EnemyView view) : base(model, view)
    {
        this.model = model;
        this.view = view;

        model.OnActiveModeChanged += HandleActiveModeChanged;
    }

    void HandleActiveModeChanged ()
    {
        switch (model.ActiveMode)
        {
            case EnemyAIMode.Chase: view.SetAsDefault(); break;
            case EnemyAIMode.Scatter: view.SetAsDefault(); break;
            case EnemyAIMode.Frightened: view.SetAsFrightened(); break;
            case EnemyAIMode.Dead: view.SetAsDead(); break;
        }
    }

    void HandleDirectionChanged ()
    {
        if (model.Direction == Direction.Left)
            view.transform.localScale = new Vector3(-1, 1, 1);
        else
            view.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void Dispose ()
    {
        model.OnDirectionChanged -= HandleDirectionChanged;
        base.Dispose();
    }
}
