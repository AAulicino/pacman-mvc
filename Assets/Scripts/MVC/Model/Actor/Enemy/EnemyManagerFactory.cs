using System.Collections.Generic;
using UnityEngine;

public static class EnemyManagerFactory
{
    public static IEnemyManager Create (
        Tile[,] map,
        IActorSettings actorSettings,
        IGameSettings gameSettings,
        ICoroutineRunner runner,
        IActorModel target,
        ICollectiblesManagerModel collectiblesManager,
        IEnemiesBehaviorSettings settings
    )
    {
        IRandomProvider random = RandomProvider.Instance;
        List<Vector2Int> startingPositions = new List<Vector2Int>();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == Tile.EnemySpawn)
                    startingPositions.Add(new Vector2Int(x, y));
            }
        }

        IEnemyAIModeManagerModel enemyModeManager = new EnemyAIModeManagerModel(runner, gameSettings);
        PathFinder pathFinder = new PathFinder(map);

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
                    startingPositions[Random.Range(0, startingPositions.Count)],
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
