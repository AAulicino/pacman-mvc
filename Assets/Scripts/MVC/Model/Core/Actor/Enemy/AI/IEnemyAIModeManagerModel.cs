using System;

public interface IEnemyAIModeManagerModel : IDisposable
{
    event Action OnActiveModeChanged;

    EnemyAIMode ActiveMode { get; }

    void Initialize ();
    void TriggerFrightenedMode ();
}
