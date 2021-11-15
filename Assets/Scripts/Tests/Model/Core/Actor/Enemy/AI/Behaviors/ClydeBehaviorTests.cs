using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Actor.Enemy.AI.Behaviors.Clyde
{
    public class ClydeBehaviorTests
    {
        IMapModel map;
        IPathFinder pathFinder;
        IRandomProvider random;
        IActorModel target;
        IClydeSettings settings;
        ICollectiblesManagerModel collectiblesManager;

        ClydeBehavior clyde;

        [SetUp]
        public void Setup ()
        {
            map = Substitute.For<IMapModel>();
            pathFinder = Substitute.For<IPathFinder>();
            random = Substitute.For<IRandomProvider>();
            target = Substitute.For<IActorModel>();
            settings = Substitute.For<IClydeSettings>();
            collectiblesManager = Substitute.For<ICollectiblesManagerModel>();

            map.Width.Returns(4);
            map.Height.Returns(4);
            map[default, default].Returns(Tile.Path);

            map.InBounds(Arg.Any<Vector2Int>()).Returns(x => InBounds(x.Arg<Vector2Int>()));
            map.InBounds(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => InBounds(x.ArgAt<int>(0), x.ArgAt<int>(1)));

            settings.DisableChaseDistance.Returns(2);

            clyde = new ClydeBehavior(map, pathFinder, random, settings, collectiblesManager);
        }

        bool InBounds (Vector2Int position) => InBounds(position.x, position.y);
        bool InBounds (int x, int y) => x >= 0 && x < map.Width && y >= 0 && y < map.Height;

        class GetAction : ClydeBehaviorTests
        {
            [Test]
            public void Stays_In_Spawn_When_CollectedCount_Smaller_Than_CollectedRequirementRatio ()
            {
                collectiblesManager.TotalCollectibles.Returns(2);
                collectiblesManager.CollectedCount.Returns(1);
                settings.CollectedRequirementRatio.Returns(0.5f);

                clyde.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.zero,
                    TileExtensions.IsEnemyWalkable
                );
            }

            [Test]
            public void Leaves_Spawn_When_CollectedCount_Greater_Than_CollectedRequirementRatio ()
            {
                collectiblesManager.TotalCollectibles.Returns(2);
                collectiblesManager.CollectedCount.Returns(2);
                settings.CollectedRequirementRatio.Returns(0.5f);

                clyde.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.zero,
                    TileExtensions.IsEnemyWalkable
                );
            }
        }

        class GetChaseAction : ClydeBehaviorTests
        {
            [Test]
            public void Chase_When_Further_Than_DisableChaseDistance ()
            {
                settings.ScatterPosition.Returns(ScatterPosition.BottomLeft);

                target.Position.Returns(Vector2Int.right * 3);

                clyde.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.right * 3,
                    TileExtensions.IsEnemyWalkable
                );
            }

            [Test]
            public void Scatter_When_Closer_Than_DisableChaseDistance ()
            {
                settings.ScatterPosition.Returns(ScatterPosition.BottomLeft);

                target.Position.Returns(Vector2Int.right);

                clyde.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                pathFinder.Received().FindPath(
                    Vector2Int.zero,
                    Vector2Int.zero,
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

                Vector2Int[] path = clyde.GetAction(Vector2Int.zero, EnemyAIMode.Chase, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetScatterAction : ClydeBehaviorTests
        {
            [Test]
            public void Returns_Scatter_Position ()
            {
                settings.ScatterPosition.Returns(ScatterPosition.BottomLeft);
                target.Position.Returns(Vector2Int.one);

                clyde.GetAction(Vector2Int.zero, EnemyAIMode.Scatter, target);

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

                Vector2Int[] path = clyde.GetAction(Vector2Int.zero, EnemyAIMode.Scatter, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetFrightenedAction : ClydeBehaviorTests
        {
            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = clyde.GetAction(Vector2Int.zero, EnemyAIMode.Frightened, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }

        class GetDeadAction : ClydeBehaviorTests
        {
            [Test]
            public void Returns_PathFinder_Path ()
            {
                target.Position.Returns(Vector2Int.zero);

                pathFinder.FindPath(default, default, default).ReturnsForAnyArgs(
                    new[] { Vector2Int.right }
                );

                Vector2Int[] path = clyde.GetAction(Vector2Int.zero, EnemyAIMode.Dead, target);

                Assert.AreEqual(Vector2Int.right, path.Last());
            }
        }
    }
}
