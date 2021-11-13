using UnityEngine;

public static class EnemyViewFactory
{
    public static EnemyView Create (EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Blinky:
                return Object.Instantiate(Resources.Load<EnemyView>("Blinky"));
            default:
                throw new System.NotImplementedException();
        }
    }
}
