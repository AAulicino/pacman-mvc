using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Actor.Enemy.AI.Behaviors.Blinky
{
    public class BlinkyBehaviorTests
    {
        IMapModel map;
        IPathFinder pathFinder;
        IRandomProvider random;
        IActorModel target;
        IBlinkySettings settings;

        BlinkyBehavior blinky;

        [SetUp]
        public void Setup ()
        {
            map = Substitute.For<IMapModel>();
            pathFinder = Substitute.For<IPathFinder>();
            random = Substitute.For<IRandomProvider>();
            target = Substitute.For<IActorModel>();
            settings = Substitute.For<IBlinkySettings>();

            map.Width.Returns(4);
            map.Height.Returns(4);
            map[default, default].Returns(Tile.Path);

            map.InBounds(Arg.Any<Vector2Int>()).Returns(x => InBounds(x.Arg<Vector2Int>()));
            map.InBounds(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => InBounds(x.ArgAt<int>(0), x.ArgAt<int>(1)));

            blinky = new BlinkyBehavior(map, pathFinder, random, settings);
        }

        bool InBounds (Vector2Int position) => InBounds(position.x, position.y);
        bool InBounds (int x, int y) => x >= 0 && x < map.Width && y >= 0 && y < map.Height;

        class GetChaseAction : BlinkyBehaviorTests
        {
            [Test]
            public void Chase_Target_Position ()
            {
                target.Position.Returns(Vector2Int.one);

                blinky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.one,
                    TileExtensions.IsEnemyWalkable
                );
            }

            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.one);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = blinky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetScatterAction : BlinkyBehaviorTests
        {
            [Test]
            public void Returns_Scatter_Position ()
            {
                settings.ScatterPosition.Returns(ScatterPosition.BottomLeft);
                target.Position.Returns(Vector2Int.one);

                blinky.GetAction(Vector2Int.zero, EnemyAIMode.Scatter, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.zero,
                    TileExtensions.IsEnemyWalkable
                );
            }

            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.one);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = blinky.GetAction(Vector2Int.zero, EnemyAIMode.Scatter, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetFrightenedAction : BlinkyBehaviorTests
        {
            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = blinky.GetAction(Vector2Int.zero, EnemyAIMode.Frightened, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetDeadAction : BlinkyBehaviorTests
        {
            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = blinky.GetAction(Vector2Int.zero, EnemyAIMode.Dead, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }
    }
}
