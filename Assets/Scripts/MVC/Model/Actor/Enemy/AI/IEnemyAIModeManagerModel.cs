using System;

public interface IEnemyAIModeManagerModel
{
    event Action OnActiveModeChanged;

    EnemyAIMode ActiveMode { get; }

    void Initialize ();
    void TriggerFrightenedMode ();
}
