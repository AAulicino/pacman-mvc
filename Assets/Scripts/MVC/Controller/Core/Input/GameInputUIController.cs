using System;

public class GameInputUIController : IDisposable
{
    readonly IGameInputModel model;
    readonly GameInputUIView[] views;

    public GameInputUIController (IGameInputModel model, GameInputUIView[] views)
    {
        this.model = model;
        this.views = views;

        model.OnDirectionChanged += HandleDirectionChange;

        foreach (GameInputUIView view in views)
            view.OnPointerDown += HandlePointerDown;

        HandleDirectionChange();
    }

    void HandlePointerDown (Direction direction)
    {
        model.SetDirection(direction);
    }

    void HandleDirectionChange ()
    {
        Direction direction = model.GetDirection();

        foreach (GameInputUIView view in views)
            view.SetSelected(view.Direction == direction);
    }

    public void Dispose ()
    {
        foreach (GameInputUIView view in views)
            view.OnPointerDown -= HandlePointerDown;
    }
}
