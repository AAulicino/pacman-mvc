using System;
using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Actor.Enemy
{
    public class EnemyModelTests
    {
        IEnemyAI ai;
        IActorSettings settings;
        ICoroutineRunner runner;
        ITimeProvider timeProvider;

        EnemyModel model;

        [SetUp]
        public void Setup ()
        {
            ai = Substitute.For<IEnemyAI>();
            settings = Substitute.For<IActorSettings>();
            runner = Substitute.For<ICoroutineRunner>();
            timeProvider = Substitute.For<ITimeProvider>();

            model = new EnemyModel(ai, settings, runner, timeProvider);
        }

        class PublicProperties : EnemyModelTests
        {
            [Test]
            public void Position ()
            {
                ai.Position.Returns(Vector2Int.one);

                Assert.AreEqual(Vector2Int.one, model.Position);
            }

            [Test]
            public void EnemyTypeProp ()
            {
                ai.EnemyType.Returns(EnemyType.Clyde);

                Assert.AreEqual(EnemyType.Clyde, model.EnemyType);
            }

            [Test]
            public void ActiveMode ()
            {
                ai.ActiveMode.Returns(EnemyAIMode.Chase);

                Assert.AreEqual(EnemyAIMode.Chase, model.ActiveMode);
            }
        }

        class MovementTime : EnemyModelTests
        {
            [Test]
            public void Equals_MovementTime_When_Not_Frightened ()
            {
                settings.MovementTime.Returns(1);
                ai.ActiveMode.Returns(EnemyAIMode.Chase);

                ai.OnActiveModeChanged += Raise.Event<Action>();

                Assert.AreEqual(1, model.MovementTime);
            }

            [Test]
            public void Equals_MovementTime_When_Frightened ()
            {
                settings.FrightenedMoveTime.Returns(1);
                ai.ActiveMode.Returns(EnemyAIMode.Frightened);

                ai.OnActiveModeChanged += Raise.Event<Action>();

                Assert.AreEqual(1, model.MovementTime);
            }
        }

        class Enable : EnemyModelTests
        {
            [Test]
            public void Raises_OnEnableChange_True ()
            {
                bool? raised = null;
                model.OnEnableChange += x => raised = x;

                model.Enable();

                Assert.IsTrue(raised);
            }

            [Test]
            public void Calls_StartCoroutine ()
            {
                model.Enable();

                runner.ReceivedWithAnyArgs().StartCoroutine(default);
            }
        }

        class Disable : EnemyModelTests
        {
            [Test]
            public void Raises_OnEnableChange_False ()
            {
                bool? raised = null;
                model.OnEnableChange += x => raised = x;

                model.Disable();

                Assert.IsFalse(raised);
            }
        }

        class Die : EnemyModelTests
        {
            [Test]
            public void Calls_AI_Die ()
            {
                model.Die();

                ai.Received().Die();
            }
        }

        class AIMove : EnemyModelTests
        {
            [Test]
            public void Calls_AI_Advance ()
            {
                timeProvider.DeltaTime.Returns(1);
                ai.OnActiveModeChanged += Raise.Event<Action>();

                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                model.Enable();

                ai.Received().Advance();
            }

            [Test]
            public void Does_Not_Call_AI_Advance_When_Time_Less_Than_MoveTime ()
            {
                timeProvider.DeltaTime.Returns(1);
                settings.MovementTime.Returns(2);
                ai.OnActiveModeChanged += Raise.Event<Action>();

                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                model.Enable();

                ai.DidNotReceive().Advance();
            }

            [Test]
            public void Changes_Direction ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                ai.Direction.Returns(Direction.Down);

                model.Enable();

                Assert.AreEqual(Direction.Down, model.Direction);
            }

            [Test]
            public void Changes_Direction_Vector ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                ai.Direction.Returns(Direction.Down);

                model.Enable();

                Assert.AreEqual(Vector2Int.down, model.DirectionVector);
            }

            [Test]
            public void Raise_DirectionChanged ()
            {
                bool? raised = null;
                model.OnDirectionChanged += () => raised = true;

                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                ai.Direction.Returns(Direction.Down);

                model.Enable();

                Assert.IsTrue(raised);
            }

            [Test]
            public void Raise_DirectionChanged_Once ()
            {
                int count = 0;
                model.OnDirectionChanged += () => count++;

                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        arg.MoveNext();
                        arg.MoveNext();
                    }
                );

                ai.Direction.Returns(Direction.Down);

                model.Enable();

                Assert.AreEqual(1, count);
            }

            [Test]
            public void Raise_OnPositionChanged ()
            {
                bool? raised = null;
                model.OnPositionChanged += () => raised = true;

                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                model.Enable();
                Assert.IsTrue(raised);
            }
        }

        class Dispose : EnemyModelTests
        {
            [Test]
            public void Raises_OnEnableChange_False ()
            {
                bool? raised = null;
                model.OnEnableChange += x => raised = x;

                model.Dispose();

                Assert.IsFalse(raised);
            }
        }
    }
}
