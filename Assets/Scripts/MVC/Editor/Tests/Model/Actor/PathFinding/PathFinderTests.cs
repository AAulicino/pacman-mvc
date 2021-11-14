using System;
using NUnit.Framework;
using UnityEngine;

public class PathFinderTests
{
    readonly Func<Tile, bool> Walkable = x => x == Tile.Path;

    PathFinder pathFinder;
    Tile[,] tiles;

    [Test]
    public void FindPath_FromBottomLeft_ToBottomRight_ReturnsPath ()
    {
        tiles = new Tile[,]
        {
            { Tile.Path, Tile.Path, Tile.Path }, // 00 01 02
            { Tile.Path, Tile.Wall, Tile.Path }, // 10 11 12
            { Tile.Path, Tile.Wall, Tile.Path }, // 20 21 22
        };

        pathFinder = new PathFinder(tiles);

        Vector2Int[] path = pathFinder.FindPath(new Vector2Int(2, 0), new Vector2Int(2, 2), Walkable);
        Assert.AreEqual(new Vector2Int(1, 0), path[0]);
        Assert.AreEqual(new Vector2Int(0, 0), path[1]);
        Assert.AreEqual(new Vector2Int(0, 1), path[2]);
        Assert.AreEqual(new Vector2Int(0, 2), path[3]);
        Assert.AreEqual(new Vector2Int(1, 2), path[4]);
        Assert.AreEqual(new Vector2Int(2, 2), path[5]);
    }

    [Test]
    public void Does_Not_Move_On_Diagonals ()
    {
        tiles = new Tile[,]
        {
            { Tile.Path, Tile.Path }, // 00 01
            { Tile.Path, Tile.Wall }  // 10 11
        };

        pathFinder = new PathFinder(tiles);

        Vector2Int[] path = pathFinder.FindPath(new Vector2Int(1, 0), new Vector2Int(0, 1), Walkable);
        Assert.AreEqual(new Vector2Int(0, 0), path[0]);
        Assert.AreEqual(new Vector2Int(0, 1), path[1]);
    }

    [Test]
    public void Does_Not_Move_On_Diagonals_2 ()
    {
        tiles = new Tile[,]
        {
            { Tile.Wall, Tile.Path }, // 00 01
            { Tile.Path, Tile.Wall }  // 10 11
        };

        pathFinder = new PathFinder(tiles);

        Vector2Int[] path = pathFinder.FindPath(new Vector2Int(1, 0), new Vector2Int(0, 1), Walkable);
        Assert.IsNull(path);
    }

    [Test]
    public void Inaccessible_Destination_Returns_Null ()
    {
        tiles = new Tile[,]
        {
            { Tile.Path, Tile.Wall, Tile.Path } // 00 01 02
        };

        pathFinder = new PathFinder(tiles);

        Vector2Int[] path = pathFinder.FindPath(new Vector2Int(0, 0), new Vector2Int(0, 2), Walkable);
        Assert.IsNull(path);
    }


    [Test]
    public void Follows_Closest_Path ()
    {
        tiles = new Tile[,]
        {
            { Tile.Path, Tile.Path, Tile.Path}, // 00 01 02
            { Tile.Path, Tile.Wall, Tile.Path}, // 11 11 12
            { Tile.Path, Tile.Path, Tile.Path}  // 20 21 22
        };

        pathFinder = new PathFinder(tiles);

        Vector2Int[] path = pathFinder.FindPath(new Vector2Int(2, 0), new Vector2Int(1, 2), Walkable);
        Assert.AreEqual(new Vector2Int(2, 1), path[0]);
        Assert.AreEqual(new Vector2Int(2, 2), path[1]);
        Assert.AreEqual(new Vector2Int(1, 2), path[2]);
    }
}
