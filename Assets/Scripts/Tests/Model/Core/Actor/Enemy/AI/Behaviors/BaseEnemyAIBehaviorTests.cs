using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Actor.Enemy.AI.Behaviors
{
    public class BaseEnemyAIBehaviorTests
    {
        IMapModel map;
        IPathFinder pathFinder;
        IRandomProvider random;
        IBaseBehaviorSettings settings;

        IActorModel target;

        DummyBaseEnemyAIBehavior behavior;

        [SetUp]
        public void Setup ()
        {
            map = Substitute.For<IMapModel>();
            pathFinder = Substitute.For<IPathFinder>();
            random = Substitute.For<IRandomProvider>();
            target = Substitute.For<IActorModel>();
            settings = Substitute.For<IBaseBehaviorSettings>();

            map.Width.Returns(4);
            map.Height.Returns(4);
            map[default, default].Returns(Tile.Path);

            map.InBounds(Arg.Any<Vector2Int>()).Returns(x => InBounds(x.Arg<Vector2Int>()));
            map.InBounds(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => InBounds(x.ArgAt<int>(0), x.ArgAt<int>(1)));

            behavior = new DummyBaseEnemyAIBehavior(map, pathFinder, random, settings);
        }

        bool InBounds (Vector2Int position) => InBounds(position.x, position.y);
        bool InBounds (int x, int y) => x >= 0 && x < map.Width && y >= 0 && y < map.Height;

        class GetDefaultDeadAction : BaseEnemyAIBehaviorTests
        {
            [Test]
            public void Returns_EnemySpawnPoint ()
            {
                map.EnemySpawnPoints.Returns(new[] { Vector2Int.left, Vector2Int.right });
                random.Range(default, default).ReturnsForAnyArgs(0);

                behavior.GetDefaultDeadAction(Vector2Int.zero, default);
            }

            [Test]
            public void Returns_EnemySpawnPoint_1 ()
            {
                map.EnemySpawnPoints.Returns(new[] { Vector2Int.left, Vector2Int.right });
                random.Range(default, default).ReturnsForAnyArgs(1);

                behavior.GetDefaultDeadAction(Vector2Int.zero, default);
            }
        }

        class GetDefaultFrightenedAction : BaseEnemyAIBehaviorTests
        {
            [Test]
            public void Returns_Away_From_Target ()
            {
                target.Position.Returns(Vector2Int.one);

                Vector2Int fleeDirection = (Vector2Int.zero - target.Position) * map.Magnitude;

                behavior.GetDefaultFrightenedAction(Vector2Int.zero, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    fleeDirection,
                    TileExtensions.IsEnemyWalkable
                );
            }
        }

        class GetDefaultScatterAction : BaseEnemyAIBehaviorTests
        {
            [Test]
            public void Moves_To_Settings_ScatterPosition ()
            {
                settings.ScatterPosition.Returns(ScatterPosition.BottomLeft);
                target.Position.Returns(Vector2Int.one);

                Vector2Int expected = new Vector2Int(0, 0);
                random.Range(0, expected.x).Returns(expected.x);
                random.Range(0, expected.y).Returns(expected.y);

                behavior.GetDefaultScatterAction(Vector2Int.zero, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    expected,
                    TileExtensions.IsEnemyWalkable
                );
            }

            [Test]
            public void Returns_PathFinder_Path ()
            {
                settings.ScatterPosition.Returns(ScatterPosition.BottomLeft);
                target.Position.Returns(Vector2Int.one);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] action = behavior.GetDefaultScatterAction(Vector2Int.zero, target);

                Assert.AreEqual(Vector2Int.right, action.Last());
            }
        }

        class GetRandomScatterPosition : BaseEnemyAIBehaviorTests
        {
            [Test]
            public void TopRight ()
            {
                Vector2Int expected = new Vector2Int(map.Width / 2, map.Height / 2);
                random.Range(expected.x, map.Width).Returns(expected.x);
                random.Range(expected.y, map.Height).Returns(expected.y);

                Vector2Int result = behavior.GetRandomScatterPosition(ScatterPosition.TopRight);

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void TopLeft ()
            {
                Vector2Int expected = new Vector2Int(0, map.Height / 2);
                random.Range(0, expected.x).Returns(expected.x);
                random.Range(expected.y, map.Height).Returns(expected.y);

                Vector2Int result = behavior.GetRandomScatterPosition(ScatterPosition.TopLeft);

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void BottomRight ()
            {
                Vector2Int expected = new Vector2Int(map.Width / 2, 0);
                random.Range(expected.x, map.Width).Returns(expected.x);
                random.Range(0, expected.y).Returns(expected.y);

                Vector2Int result = behavior.GetRandomScatterPosition(ScatterPosition.BottomRight);

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void BottomLeft ()
            {
                Vector2Int expected = new Vector2Int(0, 0);
                random.Range(0, expected.x).Returns(expected.x);
                random.Range(0, expected.y).Returns(expected.y);

                Vector2Int result = behavior.GetRandomScatterPosition(ScatterPosition.BottomLeft);

                Assert.AreEqual(expected, result);
            }
        }

        class FindPath : BaseEnemyAIBehaviorTests
        {
            [Test]
            public void Calls_PathFinder ()
            {
                behavior.FindPath(Vector2Int.zero, Vector2Int.one);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.one,
                    TileExtensions.IsEnemyWalkable
                );
            }
        }

        class GetValidPositionCloseTo : BaseEnemyAIBehaviorTests
        {
            [Test]
            public void Returns_Valid_Position ()
            {
                Vector2Int expected = new Vector2Int(3, 2);

                /*
                    1111
                    1111
                    1110
                */

                map.Width.Returns(4);
                map.Height.Returns(3);

                map[0, 0].Returns(Tile.Wall);
                map[0, 1].Returns(Tile.Wall);
                map[0, 2].Returns(Tile.Wall);

                map[1, 0].Returns(Tile.Wall);
                map[1, 1].Returns(Tile.Wall);
                map[1, 2].Returns(Tile.Wall);

                map[2, 0].Returns(Tile.Wall);
                map[2, 1].Returns(Tile.Wall);
                map[2, 2].Returns(Tile.Wall);

                map[3, 0].Returns(Tile.Wall);
                map[3, 1].Returns(Tile.Wall);
                map[3, 2].Returns(Tile.Path);

                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(1, 1));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_2 ()
            {
                Vector2Int expected = new Vector2Int(3, 2);

                /*
                    1111
                    1111
                    1110
                */

                map.Width.Returns(4);
                map.Height.Returns(3);

                map[0, 0].Returns(Tile.Wall);
                map[0, 1].Returns(Tile.Wall);
                map[0, 2].Returns(Tile.Wall);

                map[1, 0].Returns(Tile.Wall);
                map[1, 1].Returns(Tile.Wall);
                map[1, 2].Returns(Tile.Wall);

                map[2, 0].Returns(Tile.Wall);
                map[2, 1].Returns(Tile.Wall);
                map[2, 2].Returns(Tile.Wall);

                map[3, 0].Returns(Tile.Wall);
                map[3, 1].Returns(Tile.Wall);
                map[3, 2].Returns(Tile.Path);

                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(0, 0));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_Predicate ()
            {
                Vector2Int expected = new Vector2Int(3, 2);

                /*
                    1111
                    1111
                    1110
                */

                map.Width.Returns(4);
                map.Height.Returns(3);

                map[0, 0].Returns(Tile.Wall);
                map[0, 1].Returns(Tile.Wall);
                map[0, 2].Returns(Tile.Wall);

                map[1, 0].Returns(Tile.Wall);
                map[1, 1].Returns(Tile.Wall);
                map[1, 2].Returns(Tile.Wall);

                map[2, 0].Returns(Tile.Wall);
                map[2, 1].Returns(Tile.Wall);
                map[2, 2].Returns(Tile.Wall);

                map[3, 0].Returns(Tile.Wall);
                map[3, 1].Returns(Tile.Wall);
                map[3, 2].Returns(Tile.Teleport);

                Vector2Int result = behavior.GetValidPositionCloseTo(
                    new Vector2Int(1, 1),
                    x => x == Tile.Path || x == Tile.Teleport
                );

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Itself_When_Valid ()
            {
                Vector2Int expected = new Vector2Int(1, 0);

                /*
                    000
                */

                map.Width.Returns(3);
                map.Height.Returns(1);

                map[0, 0].Returns(Tile.Path);
                map[1, 0].Returns(Tile.Path);
                map[1, 0].Returns(Tile.Path);


                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(1, 0));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_OutOfBounds_Left ()
            {
                map.Width.Returns(1);
                map.Height.Returns(1);

                Vector2Int expected = new Vector2Int(0, 0);
                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(-1, 0));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_OutOfBounds_Right ()
            {
                map.Width.Returns(1);
                map.Height.Returns(1);

                Vector2Int expected = new Vector2Int(0, 0);
                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(1, 0));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_OutOfBounds_Top ()
            {
                map.Width.Returns(1);
                map.Height.Returns(1);

                Vector2Int expected = new Vector2Int(0, 0);
                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(0, 1));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_OutOfBounds_Bottom ()
            {
                map.Width.Returns(1);
                map.Height.Returns(1);

                Vector2Int expected = new Vector2Int(0, 0);
                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(0, -1));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_OutOfBounds_TopLeft ()
            {
                map.Width.Returns(1);
                map.Height.Returns(1);

                Vector2Int expected = new Vector2Int(0, 0);
                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(-1, 1));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_OutOfBounds_TopRight ()
            {
                map.Width.Returns(1);
                map.Height.Returns(1);

                Vector2Int expected = new Vector2Int(0, 0);
                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(1, 1));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_OutOfBounds_BottomLeft ()
            {
                map.Width.Returns(1);
                map.Height.Returns(1);

                Vector2Int expected = new Vector2Int(0, 0);
                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(-1, -1));

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Returns_Valid_Position_OutOfBounds_BottomRight ()
            {
                map.Width.Returns(1);
                map.Height.Returns(1);

                Vector2Int expected = new Vector2Int(0, 0);
                Vector2Int result = behavior.GetValidPositionCloseTo(new Vector2Int(1, -1));

                Assert.AreEqual(expected, result);
            }
        }

        class DummyBaseEnemyAIBehavior : BaseEnemyAIBehavior
        {
            public override EnemyType EnemyType => throw new NotImplementedException();

            public DummyBaseEnemyAIBehavior (
                IMapModel map,
                IPathFinder pathFinder,
                IRandomProvider random,
                IBaseBehaviorSettings settings
            )
                : base(map, pathFinder, random, settings)
            {
            }

            public override Vector2Int[] GetAction (Vector2Int position, EnemyAIMode mode, IActorModel target)
            {
                throw new NotImplementedException();
            }

            public new Vector2Int[] GetDefaultDeadAction (Vector2Int position, IActorModel target)
                => base.GetDefaultDeadAction(position, target);

            public new Vector2Int[] GetDefaultFrightenedAction (Vector2Int position, IActorModel target)
                => base.GetDefaultFrightenedAction(position, target);

            public new Vector2Int[] GetDefaultScatterAction (Vector2Int position, IActorModel target)
                => base.GetDefaultScatterAction(position, target);

            public new Vector2Int GetRandomScatterPosition (ScatterPosition position)
                => base.GetRandomScatterPosition(position);

            public new Vector2Int[] FindPath (Vector2Int start, Vector2Int end)
                => base.FindPath(start, end);

            public new Vector2Int GetValidPositionCloseTo (Vector2Int pos)
                => base.GetValidPositionCloseTo(pos);

            public new Vector2Int GetValidPositionCloseTo (Vector2Int pos, Func<Tile, bool> validTilePredicate)
                => base.GetValidPositionCloseTo(pos, validTilePredicate);
        }
    }
}
