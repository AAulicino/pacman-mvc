public class EnemyManager : IEnemyManager
{
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
            enemy.Enable();
        enemyModeManager.Initialize();
    }

    public void TriggerFrightenedMode ()
    {
        enemyModeManager.TriggerFrightenedMode();
    }
}
