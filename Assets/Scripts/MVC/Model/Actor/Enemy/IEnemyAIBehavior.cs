using UnityEngine;

public interface IEnemyAIBehavior
{
    EnemyType EnemyType { get; }

    Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target);
}
