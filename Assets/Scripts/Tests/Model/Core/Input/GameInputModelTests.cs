using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Input
{
    public class GameInputModelTests
    {
        GameInputModel model;

        IInputProvider inputProvider;

        [SetUp]
        public void Setup ()
        {
            inputProvider = Substitute.For<IInputProvider>();
            model = new GameInputModel(inputProvider);
        }

        class GetDirection : GameInputModelTests
        {
            [Test]
            public void Starting_Direction_Is_Left ()
            {
                Assert.AreEqual(Direction.Left, model.GetDirection());
            }

            [Test]
            public void If_Input_Left_Returns_Direction_Left ()
            {
                inputProvider.GetKey(KeyCode.LeftArrow).Returns(true);
                Assert.AreEqual(Direction.Left, model.GetDirection());
            }

            [Test]
            public void If_Input_Right_Returns_Direction_Right ()
            {
                inputProvider.GetKey(KeyCode.RightArrow).Returns(true);
                Assert.AreEqual(Direction.Right, model.GetDirection());
            }

            [Test]
            public void If_Input_Up_Returns_Direction_Up ()
            {
                inputProvider.GetKey(KeyCode.UpArrow).Returns(true);
                Assert.AreEqual(Direction.Up, model.GetDirection());
            }

            [Test]
            public void If_Input_Down_Returns_Direction_Down ()
            {
                inputProvider.GetKey(KeyCode.DownArrow).Returns(true);
                Assert.AreEqual(Direction.Down, model.GetDirection());
            }
        }

        class SetDirection : GameInputModelTests
        {
            [Test]
            public void Returns_Set_Direction_Left ()
            {
                model.SetDirection(Direction.Left);
                Assert.AreEqual(Direction.Left, model.GetDirection());
            }

            [Test]
            public void Returns_Set_Direction_Any ()
            {
                model.SetDirection((Direction)(-1));
                Assert.AreEqual((Direction)(-1), model.GetDirection());
            }

            [Test]
            public void Raises_OnDirectionChanged ()
            {
                bool? raised = null;
                model.OnDirectionChanged += () => raised = true;
                model.SetDirection(Direction.Right);
                Assert.IsTrue(raised);
            }
        }
    }
}
