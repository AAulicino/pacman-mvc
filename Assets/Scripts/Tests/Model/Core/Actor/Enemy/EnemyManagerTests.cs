using System;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Core.Actor.Enemy
{
    public class EnemyManagerTests
    {
        EnemyManager enemyManager;

        IEnemyAIModeManagerModel enemyAIModeManager;
        IEnemyModel enemy1;
        IEnemyModel enemy2;

        [SetUp]
        public void SetUp ()
        {
            enemyAIModeManager = Substitute.For<IEnemyAIModeManagerModel>();
            enemy1 = Substitute.For<IEnemyModel>();
            enemy2 = Substitute.For<IEnemyModel>();

            enemyManager = new EnemyManager(enemyAIModeManager, enemy1, enemy2);
        }

        public class Events : EnemyManagerTests
        {
            [Test]
            public void Raises_OnEnemyPositionChanged ()
            {
                enemyManager.Initialize();
                IEnemyModel enemy = null;
                enemyManager.OnEnemyPositionChanged += x => enemy = x;

                enemy1.OnPositionChanged += Raise.Event<Action>();

                Assert.AreEqual(enemy1, enemy);
            }
        }

        public class Initialize : EnemyManagerTests
        {
            [Test]
            public void Calls_Enable_On_Enemies ()
            {
                enemyManager.Initialize();

                enemy1.Received().Enable();
                enemy2.Received().Enable();
            }

            [Test]
            public void Initialize_EnemyModeManager ()
            {
                enemyManager.Initialize();

                enemyAIModeManager.Received().Initialize();
            }

            [Test]
            public void Subscribes_To_OnPositionChanged ()
            {
                enemyManager.Initialize();

                enemy1.Received().OnPositionChanged += Arg.Any<Action>();
                enemy2.Received().OnPositionChanged += Arg.Any<Action>();
            }
        }

        public class TriggerFrightenedMode : EnemyManagerTests
        {
            [Test]
            public void Calls_TriggerFrightenedMode_On_EnemyModeManager ()
            {
                enemyManager.TriggerFrightenedMode();

                enemyAIModeManager.Received().TriggerFrightenedMode();
            }
        }

        public class Disable : EnemyManagerTests
        {
            [Test]
            public void Calls_Disable_On_Enemies ()
            {
                enemyManager.Disable();

                enemy1.Received().Disable();
                enemy2.Received().Disable();
            }
        }

        public class Dispose : EnemyManagerTests
        {
            [Test]
            public void Calls_Dispose_On_Enemies ()
            {
                enemyManager.Dispose();

                enemy1.Received().Dispose();
                enemy2.Received().Dispose();
            }

            [Test]
            public void Calls_Dispose_On_EnemyModeManager ()
            {
                enemyManager.Dispose();

                enemyAIModeManager.Received().Dispose();
            }
        }
    }
}
