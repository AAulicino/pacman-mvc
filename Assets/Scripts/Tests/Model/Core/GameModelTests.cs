using System;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core
{
    public class GameModelTests
    {
        GameModel model;

        IMapModel map;
        IPlayerModel player;
        IEnemyManager enemyManager;
        ICollectiblesManagerModel collectiblesManager;

        [SetUp]
        public void Setup ()
        {
            map = Substitute.For<IMapModel>();
            player = Substitute.For<IPlayerModel>();
            enemyManager = Substitute.For<IEnemyManager>();
            collectiblesManager = Substitute.For<ICollectiblesManagerModel>();

            model = new GameModel(map, player, enemyManager, collectiblesManager);
        }

        class Initialize : GameModelTests
        {
            [Test]
            public void Calls_Player_Enable ()
            {
                model.Initialize();

                player.Received().Enable();
            }

            [Test]
            public void Calls_EnemyManger_Initialize ()
            {
                model.Initialize();

                enemyManager.Received().Initialize();
            }
        }

        class PlayerMovementCollision : GameModelTests
        {
            [Test]
            public void Player_Collide_With_Enemy_Raise_OnGameEnded_False ()
            {
                IEnemyModel enemy = Substitute.For<IEnemyModel>();
                enemyManager.Enemies.Returns(new[] { enemy });

                enemy.Position.Returns(Vector2Int.one);
                player.Position.Returns(Vector2Int.one);

                bool? called = null;
                model.OnGameEnded += x => called = x;

                player.OnPositionChanged += Raise.Event<Action>();

                Assert.IsFalse(called);
            }

            [Test]
            public void Player_PowerUp_Collide_With_Kills_Enemy ()
            {
                IEnemyModel enemy = Substitute.For<IEnemyModel>();
                enemyManager.Enemies.Returns(new[] { enemy });

                enemy.Position.Returns(Vector2Int.one);
                player.Position.Returns(Vector2Int.one);
                player.HasPowerUp.Returns(true);

                player.OnPositionChanged += Raise.Event<Action>();

                enemy.Received().Die();
            }

            [Test]
            public void Player_Collide_With_Enemy_Calls_Player_Die ()
            {
                IEnemyModel enemy = Substitute.For<IEnemyModel>();
                enemyManager.Enemies.Returns(new[] { enemy });

                enemy.Position.Returns(Vector2Int.one);
                player.Position.Returns(Vector2Int.one);

                bool? called = null;
                model.OnGameEnded += x => called = x;

                player.OnPositionChanged += Raise.Event<Action>();

                player.Received().Die();
            }
        }

        class EnemyMovementCollision : GameModelTests
        {
            [Test]
            public void Enemy_Collide_With_Player_Kills_Player ()
            {
                IEnemyModel enemy = Substitute.For<IEnemyModel>();

                enemy.Position.Returns(Vector2Int.one);
                player.Position.Returns(Vector2Int.one);

                enemyManager.OnEnemyPositionChanged += Raise.Event<Action<IEnemyModel>>(enemy);

                player.Received().Die();
            }

            [Test]
            public void Enemy_Collide_With_PowerUp_Player_Dies ()
            {
                IEnemyModel enemy = Substitute.For<IEnemyModel>();

                enemy.Position.Returns(Vector2Int.one);
                player.Position.Returns(Vector2Int.one);
                player.HasPowerUp.Returns(true);

                enemyManager.OnEnemyPositionChanged += Raise.Event<Action<IEnemyModel>>(enemy);

                enemy.Received().Die();
            }
        }

        class CollectiblesManager : GameModelTests
        {
            [Test]
            public void Equals_Collectibles ()
            {
                Assert.AreEqual(collectiblesManager, model.CollectiblesManager);
            }

            [Test]
            public void OnAllCollectiblesCollected_Raise_OnGameEnded_True ()
            {
                bool? called = null;
                model.OnGameEnded += x => called = x;

                collectiblesManager.OnAllCollectiblesCollected += Raise.Event<Action>();

                Assert.IsTrue(called);
            }

            [Test]
            public void PlayerMovement_Collects_Collectible ()
            {
                player.OnPositionChanged += Raise.Event<Action>();

                collectiblesManager.ReceivedWithAnyArgs().TryCollect(
                    default,
                    out Arg.Any<CollectibleType>()
                );
            }

            [Test]
            public void Collect_PowerUp_Calls_Player_PowerUp ()
            {
                collectiblesManager.TryCollect(
                    default,
                    out Arg.Any<CollectibleType>()
                ).ReturnsForAnyArgs(x =>
                {
                    x[1] = CollectibleType.PowerUp;
                    return true;
                });

                player.OnPositionChanged += Raise.Event<Action>();

                player.Received().PowerUp();
            }

            [Test]
            public void Collect_PowerUp_Calls_EnemyManager_TriggerFrightenedMode ()
            {
                collectiblesManager.TryCollect(
                    default,
                    out Arg.Any<CollectibleType>()
                ).ReturnsForAnyArgs(x =>
                {
                    x[1] = CollectibleType.PowerUp;
                    return true;
                });

                player.OnPositionChanged += Raise.Event<Action>();

                enemyManager.Received().TriggerFrightenedMode();
            }
        }

        class GameEnd : GameModelTests
        {
            [Test]
            public void GameEnd_Disables_Player ()
            {
                collectiblesManager.OnAllCollectiblesCollected += Raise.Event<Action>();

                player.Received().Disable();
            }

            [Test]
            public void GameEnd_Disables_Enemies ()
            {
                collectiblesManager.OnAllCollectiblesCollected += Raise.Event<Action>();

                enemyManager.Received().Disable();
            }

            [Test]
            public void GameEnd_False_Disables_Enemies ()
            {
                IEnemyModel enemy = Substitute.For<IEnemyModel>();
                enemyManager.Enemies.Returns(new[] { enemy });

                enemy.Position.Returns(Vector2Int.one);
                player.Position.Returns(Vector2Int.one);

                bool? called = null;
                model.OnGameEnded += x => called = x;

                player.OnPositionChanged += Raise.Event<Action>();

                enemyManager.Received().Disable();
            }
        }

        class Dispose : GameModelTests
        {
            [Test]
            public void Calls_Player_Dispose ()
            {
                model.Dispose();
                player.Received().Dispose();
            }

            [Test]
            public void Calls_EnemyManager_Dispose ()
            {
                model.Dispose();
                enemyManager.Received().Dispose();
            }
        }
    }
}
