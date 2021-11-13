public interface IEnemyManager
{
    IEnemyModel[] Enemies { get; }

    void Initialize ();
    void TriggerFrightenedMode ();
}
