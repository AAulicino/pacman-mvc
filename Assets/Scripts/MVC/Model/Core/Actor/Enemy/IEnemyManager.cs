using System;

public interface IEnemyManager
{
    IEnemyModel[] Enemies { get; }

    event Action<IEnemyModel> OnEnemyPositionChanged;

    void Initialize ();
    void TriggerFrightenedMode ();
}
