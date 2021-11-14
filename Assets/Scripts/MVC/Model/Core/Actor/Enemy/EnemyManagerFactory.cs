using UnityEngine;

public static class EnemyManagerFactory
{
    public static IEnemyManager Create (
        IPathFinder pathFinder,
        IMapModel map,
        IActorSettings actorSettings,
        IGameSettings gameSettings,
        ICoroutineRunner runner,
        IActorModel target,
        ICollectiblesManagerModel collectiblesManager,
        IEnemiesBehaviorSettings settings
    )
    {
        IRandomProvider random = RandomProvider.Instance;

        IEnemyAIModeManagerModel enemyModeManager = new EnemyAIModeManagerModel(runner, gameSettings);

        IEnemyModel blinky = CreateEnemyModel(
            new BlinkyBehavior(map, pathFinder, random, settings.Blinky)
        );

        IEnemyModel pinky = CreateEnemyModel(
            new PinkyBehavior(map, pathFinder, random, settings.Pinky)
        );

        IEnemyModel inky = CreateEnemyModel(
            new InkyBehavior(map, pathFinder, random, collectiblesManager, blinky, settings.Inky)
        );

        IEnemyModel clyde = CreateEnemyModel(
            new ClydeBehavior(map, pathFinder, random, collectiblesManager, settings.Clyde)
        );

        return new EnemyManager(enemyModeManager, blinky, pinky, inky, clyde);

        IEnemyModel CreateEnemyModel (IEnemyAIBehavior behavior)
        {
            return new EnemyModel(
                new EnemyAI(
                    map.EnemySpawnPoints[Random.Range(0, map.EnemySpawnPoints.Count)],
                    enemyModeManager,
                    target,
                    behavior
                ),
                actorSettings,
                runner
            );
        }
    }
}
