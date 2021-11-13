using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : IPathFinder
{
    readonly Tile[,] map;

    public PathFinder (Tile[,] map)
    {
        this.map = map;
    }

    public Vector2Int[] FindPath (Vector2Int start, Vector2Int goal)
        => FindPath(new Node(start, GetTile(start)), new Node(goal, GetTile(goal)));

    Tile GetTile (Vector2Int position) => map[position.x, position.y];

    Vector2Int[] FindPath (Node start, Node end)
    {
        List<Node> open = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();
        open.Add(start);

        while (open.Count > 0)
        {
            Node current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].Cost < current.Cost)
                    current = open[i];
            }

            open.Remove(current);
            closed.Add(current);

            if (current.Equals(end))
                return RetracePath(start, current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!neighbor.Walkable || closed.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = current.PathLength + GetDistance(current, neighbor);
                if (newMovementCostToNeighbor < neighbor.PathLength || !open.Contains(neighbor))
                {
                    neighbor.PathLength = newMovementCostToNeighbor;
                    neighbor.StraightLineLength = GetDistance(neighbor, end);
                    neighbor.Parent = current;
                    if (!open.Contains(neighbor))
                        open.Add(neighbor);
                }
            }
        }

        return null;
    }

    int GetDistance (Node neighbor, Node end)
    {
        return Mathf.Abs(neighbor.Position.x - end.Position.x)
            + Mathf.Abs(neighbor.Position.y - end.Position.y);
    }

    IEnumerable<Node> GetNeighbors (Node node)
    {
        int x = node.Position.x + 1;
        int y = node.Position.y;

        if (IsInBounds(x, y))
            yield return new Node(new Vector2Int(x, y), map[x, y]);

        x = node.Position.x - 1;
        if (IsInBounds(x, y))
            yield return new Node(new Vector2Int(x, y), map[x, y]);

        x = node.Position.x;
        y = node.Position.y + 1;
        if (IsInBounds(x, y))
            yield return new Node(new Vector2Int(x, y), map[x, y]);

        y = node.Position.y - 1;
        if (IsInBounds(x, y))
            yield return new Node(new Vector2Int(x, y), map[x, y]);
    }

    bool IsInBounds (int x, int y)
        => x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);

    Vector2Int[] RetracePath (Node start, Node end)
    {
        Vector2Int[] path = new Vector2Int[end.PathLength];
        Node current = end;

        int index = end.PathLength - 1;
        while (current != start)
        {
            path[index--] = current.Position;
            current = current.Parent;
        }
        return path;
    }

    class Node : IEquatable<Node>
    {
        public Vector2Int Position { get; }
        public bool Walkable => tile.IsEnemyWalkable();

        public int PathLength { get; set; }
        public int StraightLineLength { get; set; }
        public int Cost => PathLength + StraightLineLength;

        public Node Parent { get; set; }

        readonly Tile tile;

        public Node (Vector2Int location, Tile tile)
        {
            Position = location;
            this.tile = tile;
        }

        public bool Equals (Node other) => other.Position == Position;

        public override bool Equals (object obj) => obj is Node other && Equals(other);
        public override int GetHashCode () => Position.GetHashCode();
    }
}
