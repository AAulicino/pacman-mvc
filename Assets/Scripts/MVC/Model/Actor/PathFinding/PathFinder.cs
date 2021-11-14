using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : IPathFinder
{
    readonly Tile[,] map;
    readonly int width;
    readonly int height;

    public PathFinder (Tile[,] map)
    {
        this.map = map;
        width = map.GetLength(0);
        height = map.GetLength(1);
    }

    public Vector2Int[] FindPath (
        Vector2Int start,
        Vector2Int goal,
        Func<Tile, bool> walkablePredicate
    )
    {
        NodeFactory nodeFactory = new NodeFactory(walkablePredicate);
        return FindPath(
            nodeFactory.Create(start, map[start.x, start.y]),
            nodeFactory.Create(goal, map[start.x, start.y]),
            nodeFactory
        );
    }

    Vector2Int[] FindPath (Node start, Node end, NodeFactory nodeFactory)
    {
        HashSet<Node> open = new HashSet<Node>();
        HashSet<Node> closed = new HashSet<Node>();
        open.Add(start);

        while (open.Count > 0)
        {
            Node current = null;

            foreach (Node node in open)
            {
                if (current == null || node.Cost < current.Cost)
                    current = node;
            }

            open.Remove(current);
            closed.Add(current);

            if (current.Equals(end))
                return RetracePath(start, current);

            foreach (Node neighbor in GetNeighbors(current, nodeFactory))
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

    IEnumerable<Node> GetNeighbors (Node node, NodeFactory nodeFactory)
    {
        int x = node.Position.x;
        int y = node.Position.y;

        if (IsInBounds(x + 1, y))
            yield return CreateNode(x + 1, y, nodeFactory);

        if (IsInBounds(x - 1, y))
            yield return CreateNode(x - 1, y, nodeFactory);

        if (IsInBounds(x, y + 1))
            yield return CreateNode(x, y + 1, nodeFactory);

        if (IsInBounds(x, y - 1))
            yield return CreateNode(x, y - 1, nodeFactory);
    }

    bool IsInBounds (int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

    Node CreateNode (int x, int y, NodeFactory nodeFactory)
        => nodeFactory.Create(new Vector2Int(x, y), map[x, y]);

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

    class NodeFactory
    {
        readonly Func<Tile, bool> walkablePredicate;

        public NodeFactory (Func<Tile, bool> walkablePredicate)
        {
            this.walkablePredicate = walkablePredicate;
        }

        public Node Create (Vector2Int location, Tile tile)
        {
            return new Node(location, walkablePredicate(tile));
        }
    }

    class Node : IEquatable<Node>
    {
        public Vector2Int Position { get; }
        public bool Walkable { get; }

        public int PathLength { get; set; }
        public int StraightLineLength { get; set; }
        public int Cost => PathLength + StraightLineLength;

        public Node Parent { get; set; }

        public Node (Vector2Int location, bool walkable)
        {
            Position = location;
            Walkable = walkable;
        }

        public bool Equals (Node other) => other.Position == Position;

        public override bool Equals (object obj) => obj is Node other && Equals(other);
        public override int GetHashCode () => Position.GetHashCode();
    }
}
