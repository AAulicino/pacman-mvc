using System;
using UnityEngine;

public interface IPathFinder
{
    Vector2Int[] FindPath (
        Vector2Int start,
        Vector2Int goal,
        Func<Tile, bool> walkablePredicate
    );
}
