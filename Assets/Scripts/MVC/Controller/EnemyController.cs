public class EnemyController : ActorController
{
    readonly IEnemyModel model;
    readonly EnemyView view;

    public EnemyController (IEnemyModel model, EnemyView view) : base(model, view)
    {
        this.model = model;
        this.view = view;
    }
}
