using System;

public interface IEnemyManager : IDisposable
{
    IEnemyModel[] Enemies { get; }

    event Action<IEnemyModel> OnEnemyPositionChanged;

    void Initialize ();
    void TriggerFrightenedMode ();
}
