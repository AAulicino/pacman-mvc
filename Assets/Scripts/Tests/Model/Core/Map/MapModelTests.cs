using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Map
{
    public class MapModelTests
    {
        MapModel model;
        Tile[,] tiles;

        [SetUp]
        public void Setup ()
        {
            tiles = new Tile[,]
            {
                { Tile.Path, Tile.Path, Tile.Path }, // 00 01 02
                { Tile.Path, Tile.Wall, Tile.Path }, // 10 11 12
                { Tile.Path, Tile.Wall, Tile.Path }, // 20 21 22
                { Tile.EnemySpawn, Tile.Teleport, Tile.PlayerSpawn }, // 30 31 32
            };

            model = new MapModel(tiles);
        }

        class PublicProperties : MapModelTests
        {
            [Test]
            public void Width ()
            {
                Assert.AreEqual(tiles.GetLength(0), model.Width);
            }

            [Test]
            public void Height ()
            {
                Assert.AreEqual(tiles.GetLength(1), model.Height);
            }

            [Test]
            public void Magnitude ()
            {
                Assert.AreEqual(
                    (int)Mathf.Sqrt(model.Width * model.Width + model.Height * model.Height),
                    model.Magnitude
                );
            }

            [Test]
            public void TeleportPositions ()
            {
                Assert.AreEqual(new Vector2Int(3, 1), model.TeleportPositions[0]);
            }

            [Test]
            public void EnemySpawnPoints ()
            {
                Assert.AreEqual(new Vector2Int(3, 0), model.EnemySpawnPoints[0]);
            }

            [Test]
            public void PlayerSpawnPoint ()
            {
                Assert.AreEqual(new Vector2Int(3, 2), model.PlayerSpawnPoint);
            }
        }

        class Indexer : MapModelTests
        {
            [Test]
            public void Get_With_Two_Ints ()
            {
                Assert.AreEqual(Tile.Path, model[0, 0]);
                Assert.AreEqual(Tile.Wall, model[1, 1]);
                Assert.AreEqual(Tile.Path, model[2, 2]);
            }

            [Test]
            public void Get_With_Vector2Int ()
            {
                Assert.AreEqual(Tile.Path, model[new Vector2Int(0, 0)]);
                Assert.AreEqual(Tile.Wall, model[new Vector2Int(1, 1)]);
                Assert.AreEqual(Tile.Path, model[new Vector2Int(2, 2)]);
            }
        }

        class InBounds : MapModelTests
        {
            [Test]
            public void True_With_Two_Ints ()
            {
                Assert.IsTrue(model.InBounds(0, 0));
                Assert.IsTrue(model.InBounds(1, 1));
                Assert.IsTrue(model.InBounds(2, 2));
            }

            [Test]
            public void True_With_Vector2Int ()
            {
                Assert.IsTrue(model.InBounds(new Vector2Int(0, 0)));
                Assert.IsTrue(model.InBounds(new Vector2Int(1, 1)));
                Assert.IsTrue(model.InBounds(new Vector2Int(2, 2)));
            }

            [Test]
            public void False_With_Two_Ints ()
            {
                Assert.IsFalse(model.InBounds(-1, 0));
                Assert.IsFalse(model.InBounds(0, -1));
                Assert.IsFalse(model.InBounds(4, 0));
                Assert.IsFalse(model.InBounds(0, 3));
            }

            [Test]
            public void False_With_Vector2Int ()
            {
                Assert.IsFalse(model.InBounds(new Vector2Int(-1, 0)));
                Assert.IsFalse(model.InBounds(new Vector2Int(0, -1)));
                Assert.IsFalse(model.InBounds(new Vector2Int(4, 0)));
                Assert.IsFalse(model.InBounds(new Vector2Int(0, 3)));
            }
        }

    }
}
