using System;

public class EnemyManager : IEnemyManager
{
    public event Action<IEnemyModel> OnEnemyPositionChanged;

    public IEnemyModel[] Enemies => enemies;

    readonly IEnemyModel[] enemies;
    readonly IEnemyAIModeManagerModel enemyModeManager;

    public EnemyManager (IEnemyAIModeManagerModel enemyModeManager, params IEnemyModel[] enemies)
    {
        this.enemies = enemies;
        this.enemyModeManager = enemyModeManager;
    }

    public void Initialize ()
    {
        foreach (IEnemyModel enemy in enemies)
        {
            enemy.OnPositionChanged += () => OnEnemyPositionChanged(enemy);
            enemy.Enable();
        }
        enemyModeManager.Initialize();
    }

    public void TriggerFrightenedMode ()
    {
        enemyModeManager.TriggerFrightenedMode();
    }

    public void Disable ()
    {
        foreach (IEnemyModel enemy in Enemies)
            enemy.Disable();
    }

    public void Dispose ()
    {
        foreach (IEnemyModel enemy in enemies)
            enemy.Dispose();
        enemyModeManager.Dispose();
    }
}
