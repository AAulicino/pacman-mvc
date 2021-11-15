using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Actor.Enemy.AI.Behaviors.Inky
{
    public class InkyBehaviorTests
    {
        IMapModel map;
        IPathFinder pathFinder;
        IRandomProvider random;
        IActorModel target;
        IInkySettings settings;
        ICollectiblesManagerModel collectiblesManager;
        IEnemyModel blinky;

        InkyBehavior inky;

        [SetUp]
        public void Setup ()
        {
            map = Substitute.For<IMapModel>();
            pathFinder = Substitute.For<IPathFinder>();
            random = Substitute.For<IRandomProvider>();
            target = Substitute.For<IActorModel>();
            settings = Substitute.For<IInkySettings>();
            collectiblesManager = Substitute.For<ICollectiblesManagerModel>();
            blinky = Substitute.For<IEnemyModel>();

            map.Width.Returns(4);
            map.Height.Returns(4);
            map[default, default].Returns(Tile.Path);
            map.EnemySpawnPoints.Returns(new[] { Vector2Int.zero, Vector2Int.right });

            map.InBounds(Arg.Any<Vector2Int>()).Returns(x => InBounds(x.Arg<Vector2Int>()));
            map.InBounds(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => InBounds(x.ArgAt<int>(0), x.ArgAt<int>(1)));

            inky = new InkyBehavior(map, pathFinder, random, settings, collectiblesManager, blinky);
        }

        bool InBounds (Vector2Int position) => InBounds(position.x, position.y);
        bool InBounds (int x, int y) => x >= 0 && x < map.Width && y >= 0 && y < map.Height;

        class EnemyTypeProperty : InkyBehaviorTests
        {
            [Test]
            public void Equals_Inky ()
            {
                Assert.AreEqual(EnemyType.Inky, inky.EnemyType);
            }
        }

        class GetAction : InkyBehaviorTests
        {
            [Test]
            public void Stays_In_Spawn_When_CollectedCount_Smaller_Than_CollectedRequirement ()
            {
                collectiblesManager.CollectedCount.Returns(0);
                settings.CollectedRequirement.Returns(2);

                inky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.zero,
                    TileExtensions.IsEnemyWalkable
                );
            }
        }

        class GetChaseAction : InkyBehaviorTests
        {
            [Test]
            public void Chase_Leads_Target_Position ()
            {
                collectiblesManager.CollectedCount.Returns(1);
                settings.LeadingTilesAheadOfPacman.Returns(2);
                blinky.Position.Returns(Vector2Int.one);

                target.Position.Returns(Vector2Int.zero);
                target.Direction.Returns(Direction.Right);
                target.DirectionVector.Returns(Vector2Int.right);

                inky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                Vector2Int expected = Vector2Int.right * 2;
                expected += (expected - blinky.Position) * 2;

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.right * 3,
                    TileExtensions.IsEnemyWalkable
                );
            }

            [Test]
            public void Chase_Leads_Target_Position_Replicates_Direction_Up_Bug ()
            {
                collectiblesManager.CollectedCount.Returns(1);
                settings.LeadingTilesAheadOfPacman.Returns(2);
                blinky.Position.Returns(Vector2Int.one);

                target.Position.Returns(Vector2Int.one);
                target.Direction.Returns(Direction.Up);
                target.DirectionVector.Returns(Vector2Int.up);

                inky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                Vector2Int expected = Vector2Int.right * 2;
                expected += Vector2Int.left * 2;
                expected += (expected - blinky.Position) * 2;

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.up * 3,
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

                Vector2Int[] path = inky.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetScatterAction : InkyBehaviorTests
        {
            [Test]
            public void Returns_Scatter_Position ()
            {
                settings.ScatterPosition.Returns(ScatterPosition.BottomLeft);
                target.Position.Returns(Vector2Int.one);

                inky.GetAction(Vector2Int.zero, EnemyAIMode.Scatter, target);

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

                Vector2Int[] path = inky.GetAction(Vector2Int.zero, EnemyAIMode.Scatter, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetFrightenedAction : InkyBehaviorTests
        {
            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = inky.GetAction(Vector2Int.zero, EnemyAIMode.Frightened, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetDeadAction : InkyBehaviorTests
        {
            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = inky.GetAction(Vector2Int.zero, EnemyAIMode.Dead, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }
    }
}
