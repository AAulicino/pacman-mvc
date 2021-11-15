using System;

public class GameInputUIController : IDisposable
{
    readonly IGameInputModel model;
    readonly GameInputUIView[] views;

    public GameInputUIController (IGameInputModel model, GameInputUIView[] views)
    {
        this.model = model;
        this.views = views;

        foreach (GameInputUIView view in views)
            view.OnPointerDown += HandlePointerDown;
    }

    void HandlePointerDown (Direction direction)
    {
        model.SetDirection(direction);
    }

    public void Dispose ()
    {
        foreach (GameInputUIView view in views)
            view.OnPointerDown -= HandlePointerDown;
    }
}
