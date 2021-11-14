public class PlayerController : ActorController
{
    readonly IPlayerModel model;
    readonly PlayerView view;

    public PlayerController (IPlayerModel model, PlayerView view) : base(model, view)
    {
        this.model = model;
        this.view = view;
    }
}
