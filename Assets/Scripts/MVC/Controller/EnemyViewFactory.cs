using UnityEngine;

public static class EnemyViewFactory
{
    public static EnemyView Create (EnemyType enemyType)
    {
        return Object.Instantiate(Resources.Load<EnemyView>("Enemies/" + enemyType.ToString()));
    }
}
