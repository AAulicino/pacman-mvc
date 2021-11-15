using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Collectibles
{
    public class CollectiblesManagerModelTests
    {
        CollectiblesManagerModel model;
        IMapModel mapModel;
        ICollectibleModelFactory factory;

        [SetUp]
        public void Setup ()
        {
            mapModel = Substitute.For<IMapModel>();
            factory = Substitute.For<ICollectibleModelFactory>();
            model = new CollectiblesManagerModel(mapModel, factory);
        }

        class PublicProperties : CollectiblesManagerModelTests
        {
            [Test]
            public void TotalCollectibles ()
            {
                mapModel.Width.Returns(2);
                mapModel.Height.Returns(2);

                mapModel[0, 0].Returns(Tile.Path);
                mapModel[0, 1].Returns(Tile.Path);
                mapModel[1, 0].Returns(Tile.Wall);
                mapModel[1, 1].Returns(Tile.Path);

                model.Initialize();

                Assert.AreEqual(3, model.TotalCollectibles);
            }

            [Test]
            public void CollectedCount ()
            {
                mapModel.Width.Returns(1);
                mapModel.Height.Returns(1);

                mapModel[0, 0].Returns(Tile.Path);

                model.Initialize();

                Assert.AreEqual(0, model.CollectedCount);
            }

            [Test]
            public void CollectedCount_Collected_All ()
            {
                mapModel.Width.Returns(1);
                mapModel.Height.Returns(1);

                mapModel[0, 0].Returns(Tile.Path);

                model.Initialize();

                model.TryCollect(new Vector2Int(0, 0), out CollectibleType _);

                Assert.AreEqual(1, model.CollectedCount);
            }
        }

        class TryCollect : CollectiblesManagerModelTests
        {
            [Test]
            public void TryCollect_Valid_Returns_True ()
            {
                mapModel.Width.Returns(1);
                mapModel.Height.Returns(1);

                mapModel[0, 0].Returns(Tile.Path);

                model.Initialize();

                bool result = model.TryCollect(new Vector2Int(0, 0), out CollectibleType _);

                Assert.IsTrue(result);
            }

            [Test]
            public void TryCollect_Invalid_Returns_False ()
            {
                mapModel.Width.Returns(1);
                mapModel.Height.Returns(1);

                mapModel[0, 0].Returns(Tile.Wall);

                model.Initialize();

                bool result = model.TryCollect(new Vector2Int(0, 0), out CollectibleType _);

                Assert.IsFalse(result);
            }

            [Test]
            public void TryCollect_Valid_Outputs_Default ()
            {
                mapModel.Width.Returns(1);
                mapModel.Height.Returns(1);

                mapModel[0, 0].Returns(Tile.Path);

                model.Initialize();

                model.TryCollect(new Vector2Int(0, 0), out CollectibleType type);

                Assert.AreEqual(CollectibleType.Default, type);
            }

            [Test]
            public void TryCollect_Valid_Outputs_PowerUp ()
            {
                ICollectibleModel collectible = Substitute.For<ICollectibleModel>();
                factory.Create(default, default).ReturnsForAnyArgs(collectible);
                collectible.Type.Returns(CollectibleType.PowerUp);

                mapModel.Width.Returns(1);
                mapModel.Height.Returns(1);

                mapModel[0, 0].Returns(Tile.PowerUp);

                model.Initialize();

                model.TryCollect(new Vector2Int(0, 0), out CollectibleType type);

                Assert.AreEqual(CollectibleType.PowerUp, type);
            }

            [Test]
            public void TryCollect_Valid_Raises_OnCollect ()
            {
                ICollectibleModel expected = Substitute.For<ICollectibleModel>();
                factory.Create(default, default).ReturnsForAnyArgs(expected);

                mapModel.Width.Returns(1);
                mapModel.Height.Returns(1);

                mapModel[0, 0].Returns(Tile.Path);
                model.Initialize();

                ICollectibleModel current = null;
                model.OnCollect += x => current = x;

                model.TryCollect(new Vector2Int(0, 0), out CollectibleType _);

                Assert.AreEqual(expected, current);
            }

            [Test]
            public void TryCollect_Last_Valid_Raises_OnAllCollectiblesCollected ()
            {
                ICollectibleModel expected = Substitute.For<ICollectibleModel>();
                factory.Create(default, default).ReturnsForAnyArgs(expected);

                mapModel.Width.Returns(1);
                mapModel.Height.Returns(1);

                mapModel[0, 0].Returns(Tile.Path);
                model.Initialize();

                bool? called = null;
                model.OnAllCollectiblesCollected += () => called = true;

                model.TryCollect(new Vector2Int(0, 0), out CollectibleType _);

                Assert.IsTrue(called);
            }

            [Test]
            public void TryCollect_Not_Last_Valid_Raises_OnAllCollectiblesCollected ()
            {
                mapModel.Width.Returns(2);
                mapModel.Height.Returns(2);

                mapModel[0, 0].Returns(Tile.Path);
                model.Initialize();

                bool? called = null;
                model.OnAllCollectiblesCollected += () => called = true;

                model.TryCollect(new Vector2Int(0, 0), out CollectibleType _);

                Assert.IsNull(called);
            }
        }
    }
}
