using System;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Actor.Enemy.AI
{
    public class EnemyAITests
    {
        readonly Vector2Int startingPosition = Vector2Int.zero;

        IEnemyAIModeManagerModel modeManager;
        IActorModel target;
        IEnemyAIBehavior behavior;

        EnemyAI enemyAI;

        [SetUp]
        public void Setup ()
        {
            modeManager = Substitute.For<IEnemyAIModeManagerModel>();
            target = Substitute.For<IActorModel>();
            behavior = Substitute.For<IEnemyAIBehavior>();

            enemyAI = new EnemyAI(startingPosition, modeManager, target, behavior);
        }

        class PublicProperties : EnemyAITests
        {
            [Test]
            public void Equals_EnemyType ()
            {
                behavior.EnemyType.Returns(EnemyType.Pinky);

                Assert.AreEqual(EnemyType.Pinky, enemyAI.EnemyType);
            }

            [Test]
            public void Position_Equals_StartingPosition ()
            {
                enemyAI = new EnemyAI(Vector2Int.one, default, default, default);
                Assert.AreEqual(Vector2Int.one, enemyAI.Position);
            }
        }

        class Initialize : EnemyAITests
        {
            [Test]
            public void Listens_To_OnActiveModeChanged ()
            {
                enemyAI.Initialize();
                bool? called = null;
                enemyAI.OnActiveModeChanged += () => called = true;

                modeManager.OnActiveModeChanged += Raise.Event<Action>();

                Assert.IsTrue(called);
            }
        }

        class Advance : EnemyAITests
        {
            [Test]
            public void Syncs_ActiveMode_WithManager ()
            {
                modeManager.ActiveMode.Returns(EnemyAIMode.Chase);
                enemyAI.Advance();

                Assert.AreEqual(EnemyAIMode.Chase, enemyAI.ActiveMode);
            }

            [Test]
            public void Get_Behavior_Action ()
            {
                modeManager.ActiveMode.Returns(EnemyAIMode.Chase);

                enemyAI.Advance();

                behavior.Received().GetAction(enemyAI.Position, EnemyAIMode.Chase, target);
            }

            [Test]
            public void Changes_Position ()
            {
                behavior.GetAction(default, default, default).ReturnsForAnyArgs(new[]{
                    Vector2Int.one
                });

                enemyAI.Advance();

                Assert.AreEqual(Vector2Int.one, enemyAI.Position);
            }

            [Test]
            public void Changes_Position_2 ()
            {
                modeManager.ActiveMode.Returns(EnemyAIMode.Scatter);

                behavior.GetAction(default, default, default).ReturnsForAnyArgs(new[]{
                    Vector2Int.one, Vector2Int.right
                });

                enemyAI.Advance();
                enemyAI.Advance();

                Assert.AreEqual(Vector2Int.right, enemyAI.Position);
            }

            [Test]
            public void Changes_Direction_2 ()
            {
                modeManager.ActiveMode.Returns(EnemyAIMode.Scatter);

                behavior.GetAction(default, default, default).ReturnsForAnyArgs(new[]{
                    Vector2Int.zero, Vector2Int.right
                });

                enemyAI.Advance();
                enemyAI.Advance();

                Assert.AreEqual(Direction.Right, enemyAI.Direction);
            }

            [Test]
            public void Changes_Direction_Right ()
            {
                behavior.GetAction(default, default, default).ReturnsForAnyArgs(new[]{
                    Vector2Int.right
                });

                enemyAI.Advance();

                Assert.AreEqual(Direction.Right, enemyAI.Direction);
            }

            [Test]
            public void Changes_Direction_Left ()
            {
                behavior.GetAction(default, default, default).ReturnsForAnyArgs(new[]{
                    Vector2Int.left
                });

                enemyAI.Advance();

                Assert.AreEqual(Direction.Left, enemyAI.Direction);
            }

            [Test]
            public void Changes_Direction_Up ()
            {
                behavior.GetAction(default, default, default).ReturnsForAnyArgs(new[]{
                    Vector2Int.up
                });

                enemyAI.Advance();

                Assert.AreEqual(Direction.Up, enemyAI.Direction);
            }

            [Test]
            public void Changes_Direction_Down ()
            {
                behavior.GetAction(default, default, default).ReturnsForAnyArgs(new[]{
                    Vector2Int.down
                });

                enemyAI.Advance();

                Assert.AreEqual(Direction.Down, enemyAI.Direction);
            }
        }

        class ActiveModeChanged : EnemyAITests
        {
            [Test]
            public void Get_Action_OnModeChanged ()
            {
                enemyAI.Initialize();

                modeManager.ActiveMode.Returns(EnemyAIMode.Scatter);
                modeManager.OnActiveModeChanged += Raise.Event<Action>();

                behavior.Received().GetAction(enemyAI.Position, EnemyAIMode.Scatter, target);
            }

            [Test]
            public void Does_Not_Get_Action_OnModeChanged_When_Dead ()
            {
                enemyAI.Initialize();
                enemyAI.Die();

                modeManager.ActiveMode.Returns(EnemyAIMode.Scatter);
                modeManager.OnActiveModeChanged += Raise.Event<Action>();

                behavior.DidNotReceive().GetAction(enemyAI.Position, EnemyAIMode.Scatter, target);
            }
        }

        class Die : EnemyAITests
        {
            [Test]
            public void ActiveMode_Changes_To_Dead ()
            {
                enemyAI.Die();

                Assert.AreEqual(EnemyAIMode.Dead, enemyAI.ActiveMode);
            }

            [Test]
            public void Gets_Dead_Action_From_Behavior ()
            {
                enemyAI.Die();

                behavior.Received().GetAction(enemyAI.Position, EnemyAIMode.Dead, target);
            }

            [Test]
            public void Raises_OnActiveModeChanged ()
            {
                bool? called = null;
                enemyAI.OnActiveModeChanged += () => called = true;

                enemyAI.Die();

                Assert.IsTrue(called);
            }
        }
    }
}
