using System.Collections;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Core.Actor.Enemy
{
    public class EnemyAIModeManagerModelTests
    {
        EnemyAIModeManagerModel model;
        ICoroutineRunner runner;
        IGameSettings settings;

        [SetUp]
        public void SetUp ()
        {
            runner = Substitute.For<ICoroutineRunner>();
            settings = Substitute.For<IGameSettings>();

            model = new EnemyAIModeManagerModel(runner, settings);
        }

        public class Initialize : EnemyAIModeManagerModelTests
        {
            [Test]
            public void Calls_StartCoroutine ()
            {
                model.Initialize();

                runner.ReceivedWithAnyArgs().StartCoroutine(default);
            }
        }

        public class ModeRoutine : EnemyAIModeManagerModelTests
        {
            [Test]
            public void Changes_To_ScatterMode ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                model.Initialize();

                Assert.AreEqual(EnemyAIMode.Scatter, model.ActiveMode);
            }

            [Test]
            public void Raises_OnActiveModeChanged ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );
                bool? raised = null;
                model.OnActiveModeChanged += () => raised = true;

                model.Initialize();

                Assert.IsTrue(raised);
            }

            [Test]
            public void Changes_To_ChaseMode ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        arg.MoveNext();
                    }
                );

                model.Initialize();

                Assert.AreEqual(EnemyAIMode.Chase, model.ActiveMode);
            }

            [Test]
            public void Raises_OnActiveModeChanged_2 ()
            {
                bool? raised = null;
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        model.OnActiveModeChanged += () => raised = true;
                        arg.MoveNext();
                    }
                );

                model.Initialize();

                Assert.IsTrue(raised);
            }
        }

        public class TriggerFrightenedMode : EnemyAIModeManagerModelTests
        {
            [Test]
            public void Calls_StartCoroutine ()
            {
                model.TriggerFrightenedMode();

                runner.ReceivedWithAnyArgs().StartCoroutine(default);
            }

            [Test]
            public void Raises_OnActiveModeChanged ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );
                bool? raised = null;
                model.OnActiveModeChanged += () => raised = true;

                model.TriggerFrightenedMode();

                Assert.IsTrue(raised);
            }

            [Test]
            public void Changes_To_FrightenedMode ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        arg.MoveNext();
                    }
                );

                model.TriggerFrightenedMode();

                Assert.AreEqual(EnemyAIMode.Frightened, model.ActiveMode);
            }

            [Test]
            public void Changes_To_ScatterMode ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        arg.MoveNext();
                        IEnumerator subRoutine = (IEnumerator)arg.Current;
                        subRoutine.MoveNext();
                    }
                );

                model.TriggerFrightenedMode();

                Assert.AreEqual(EnemyAIMode.Scatter, model.ActiveMode);
            }

            [Test]
            public void Raises_OnActiveModeChanged_2 ()
            {
                bool? raised = null;
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        arg.MoveNext();
                        model.OnActiveModeChanged += () => raised = true;
                        IEnumerator subRoutine = (IEnumerator)arg.Current;
                        subRoutine.MoveNext();
                    }
                );

                model.TriggerFrightenedMode();
                Assert.IsTrue(raised);
            }

            [Test]
            public void Changes_To_ChaseMode ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        arg.MoveNext();
                        IEnumerator subRoutine = (IEnumerator)arg.Current;
                        subRoutine.MoveNext();
                        subRoutine.MoveNext();
                    }
                );

                model.TriggerFrightenedMode();

                Assert.AreEqual(EnemyAIMode.Chase, model.ActiveMode);

            }
            [Test]
            public void Changes_To_ScatterMode_2 ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        arg.MoveNext();
                        IEnumerator subRoutine = (IEnumerator)arg.Current;
                        subRoutine.MoveNext();
                        subRoutine.MoveNext();
                        subRoutine.MoveNext();
                    }
                );

                model.TriggerFrightenedMode();

                Assert.AreEqual(EnemyAIMode.Scatter, model.ActiveMode);
            }
        }

        public class Dispose : EnemyAIModeManagerModelTests
        {
            [Test]
            public void Calls_StopCoroutine ()
            {
                model.Dispose();

                runner.ReceivedWithAnyArgs().StopCoroutine(default);
            }
        }
    }
}
