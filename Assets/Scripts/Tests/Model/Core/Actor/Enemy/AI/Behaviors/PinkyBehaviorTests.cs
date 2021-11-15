using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Actor.Enemy.AI.Behaviors.Pinky
{
    public class PinkyBehaviorTests
    {
        IMapModel map;
        IPathFinder pathFinder;
        IRandomProvider random;
        IActorModel target;
        IPinkySettings settings;

        PinkyBehavior pinky;

        [SetUp]
        public void Setup ()
        {
            map = Substitute.For<IMapModel>();
            pathFinder = Substitute.For<IPathFinder>();
            random = Substitute.For<IRandomProvider>();
            target = Substitute.For<IActorModel>();
            settings = Substitute.For<IPinkySettings>();

            map.Width.Returns(4);
            map.Height.Returns(4);
            map[default, default].Returns(Tile.Path);

            map.InBounds(Arg.Any<Vector2Int>()).Returns(x => InBounds(x.Arg<Vector2Int>()));
            map.InBounds(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => InBounds(x.ArgAt<int>(0), x.ArgAt<int>(1)));

            pinky = new PinkyBehavior(map, pathFinder, random, settings);
        }

        bool InBounds (Vector2Int position) => InBounds(position.x, position.y);
        bool InBounds (int x, int y) => x >= 0 && x < map.Width && y >= 0 && y < map.Height;

        class EnemyTypeProperty : PinkyBehaviorTests
        {
            [Test]
            public void Equals_Pinky ()
            {
                Assert.AreEqual(EnemyType.Pinky, pinky.EnemyType);
            }
        }

        class GetChaseAction : PinkyBehaviorTests
        {
            [Test]
            public void Chase_Leads_Target_Position ()
            {
                settings.LeadingTilesAheadOfPacman.Returns(2);
                target.Position.Returns(Vector2Int.zero);
                target.Direction.Returns(Direction.Right);
                target.DirectionVector.Returns(Vector2Int.right);

                pinky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.right * 2,
                    TileExtensions.IsEnemyWalkable
                );
            }

            [Test]
            public void Chase_Leads_Target_Position_Replicates_Direction_Up_Bug ()
            {
                settings.LeadingTilesAheadOfPacman.Returns(2);
                target.Position.Returns(Vector2Int.one * 2);
                target.Direction.Returns(Direction.Up);
                target.DirectionVector.Returns(Vector2Int.up);

                pinky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    new Vector2Int(0, 3),
                    TileExtensions.IsEnemyWalkable
                );
            }

            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = pinky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetScatterAction : PinkyBehaviorTests
        {
            [Test]
            public void Returns_Scatter_Position ()
            {
                settings.ScatterPosition.Returns(ScatterPosition.BottomLeft);
                target.Position.Returns(Vector2Int.one);

                pinky.GetAction(Vector2Int.zero, EnemyAIMode.Scatter, target);

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

                Vector2Int[] path = pinky.GetAction(Vector2Int.zero, EnemyAIMode.Scatter, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetFrightenedAction : PinkyBehaviorTests
        {
            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = pinky.GetAction(Vector2Int.zero, EnemyAIMode.Frightened, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetDeadAction : PinkyBehaviorTests
        {
            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = pinky.GetAction(Vector2Int.zero, EnemyAIMode.Dead, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }
    }
}
