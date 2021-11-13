using System.Collections.Generic;
using UnityEngine;

public static class EnemyManagerFactory
{
    public static IEnemyManager Create (
        Tile[,] map,
        IActorSettings actorSettings,
        IGameSettings gameSettings,
        ICoroutineRunner runner,
        IActorModel target
    )
    {
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

        IEnemyModel blinky = new EnemyModel(
            new EnemyAI(
                startingPositions[Random.Range(0, startingPositions.Count)],
                enemyModeManager,
                target,
                new BlinkyBehavior(map, pathFinder)
            ),
            actorSettings,
            runner
        );

        return new EnemyManager(enemyModeManager, blinky);
    }
}
