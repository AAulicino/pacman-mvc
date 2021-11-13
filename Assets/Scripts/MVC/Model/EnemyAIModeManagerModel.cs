using System;
using System.Collections;
using UnityEngine;

public class EnemyAIModeManagerModel : IEnemyAIModeManagerModel
{
    public event Action OnActiveModeChanged;

    public EnemyAIMode ActiveMode { get; private set; }

    readonly ICoroutineRunner runner;

    readonly WaitForSeconds chaseDuration;
    readonly WaitForSeconds scatterDuration;
    readonly WaitForSeconds frightenedDuration;

    Coroutine modeRoutine;

    public EnemyAIModeManagerModel (ICoroutineRunner runner, IGameSettings settings)
    {
        this.runner = runner;

        chaseDuration = new WaitForSeconds(settings.ChaseDuration);
        scatterDuration = new WaitForSeconds(settings.ScatterDuration);
        frightenedDuration = new WaitForSeconds(settings.FrightenedDuration);
    }

    public void Initialize ()
    {
        modeRoutine = runner.StartCoroutine(ModeRoutine());
    }

    public void TriggerFrightenedMode ()
    {
        if (modeRoutine != null)
            modeRoutine = runner.StartCoroutine(ModeRoutine());
    }

    IEnumerator ModeRoutine ()
    {
        while (true)
        {
            ActiveMode = EnemyAIMode.Scatter;
            OnActiveModeChanged?.Invoke();
            yield return scatterDuration;

            ActiveMode = EnemyAIMode.Chase;
            OnActiveModeChanged?.Invoke();
            yield return chaseDuration;
        }
    }

    IEnumerator FrightenedRoutine ()
    {
        ActiveMode = EnemyAIMode.Frightened;
        OnActiveModeChanged?.Invoke();
        yield return frightenedDuration;
        yield return ModeRoutine();
    }
}
