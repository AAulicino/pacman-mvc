using System;
using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Actor.Player
{
    public class PlayerModelTests
    {
        IMapModel map;
        ICoroutineRunner runner;
        IActorSettings settings;
        IGameSettings gameSettings;
        IInputProvider input;
        ITimeProvider timeProvider;

        PlayerModel model;

        [SetUp]
        public void Setup ()
        {
            map = Substitute.For<IMapModel>();
            runner = Substitute.For<ICoroutineRunner>();
            settings = Substitute.For<IActorSettings>();
            gameSettings = Substitute.For<IGameSettings>();
            input = Substitute.For<IInputProvider>();
            timeProvider = Substitute.For<ITimeProvider>();

            map[default].ReturnsForAnyArgs(Tile.Path);
            map[default, default].ReturnsForAnyArgs(Tile.Path);
            map.InBounds(default).ReturnsForAnyArgs(true);

            model = new PlayerModel(map, runner, settings, gameSettings, input, timeProvider);
        }

        class MovementTime : PlayerModelTests
        {
            [Test]
            public void Equals_Settings_MovementTime ()
            {
                settings.MovementTime.Returns(1);
                Assert.AreEqual(1, model.MovementTime);
            }
        }

        class Enable : PlayerModelTests
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

        class Disable : PlayerModelTests
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

        class Die : PlayerModelTests
        {
            [Test]
            public void Raises_OnEnableChange_False ()
            {
                bool? raised = null;
                model.OnEnableChange += x => raised = x;

                model.Die();

                Assert.IsFalse(raised);
            }
        }

        class HasPowerUp : PlayerModelTests
        {
            [Test]
            public void True ()
            {
                model.PowerUp();

                Assert.IsTrue(model.HasPowerUp);
            }

            [Test]
            public void Initial_State_False ()
            {
                Assert.IsFalse(model.HasPowerUp);
            }

            [Test]
            public void Expires_After_Duration ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                        arg.MoveNext();
                    }
                );

                gameSettings.PowerUpDuration.Returns(2);

                model.PowerUp();

                Assert.IsFalse(model.HasPowerUp);
            }
        }

        class Move : PlayerModelTests
        {
            [Test]
            public void Changes_Direction_Input_Up ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                input.GetKey(KeyCode.UpArrow).Returns(true);

                model.Enable();

                Assert.AreEqual(Direction.Up, model.Direction);
                Assert.AreEqual(Vector2Int.up, model.DirectionVector);
            }

            [Test]
            public void Changes_Direction_Input_Down ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                input.GetKey(KeyCode.DownArrow).Returns(true);

                model.Enable();

                Assert.AreEqual(Direction.Down, model.Direction);
                Assert.AreEqual(Vector2Int.down, model.DirectionVector);
            }

            [Test]
            public void Changes_Direction_Input_Left ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                input.GetKey(KeyCode.LeftArrow).Returns(true);

                model.Enable();

                Assert.AreEqual(Direction.Left, model.Direction);
                Assert.AreEqual(Vector2Int.left, model.DirectionVector);
            }

            [Test]
            public void Changes_Direction_Input_Right ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                input.GetKey(KeyCode.RightArrow).Returns(true);

                model.Enable();

                Assert.AreEqual(Vector2Int.right, model.DirectionVector);
                Assert.AreEqual(Direction.Right, model.Direction);
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

                input.GetKey(KeyCode.DownArrow).Returns(true);

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

                input.GetKey(KeyCode.DownArrow).Returns(true);

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

            [Test]
            public void If_Hit_Wall_Does_Not_Change_Direction ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        input.GetKey(KeyCode.UpArrow).Returns(true);
                        arg.MoveNext();
                        input.GetKey(KeyCode.UpArrow).Returns(false);
                        input.GetKey(KeyCode.RightArrow).Returns(true);
                        arg.MoveNext();
                    }
                );

                map[Vector2Int.up * 2].Returns(Tile.Wall);
                map[Vector2Int.up + Vector2Int.right].Returns(Tile.Wall);

                model.Enable();

                Assert.AreEqual(Vector2Int.up, model.DirectionVector);
                Assert.AreEqual(Direction.Up, model.Direction);
            }

            [Test]
            public void If_Hit_Wall_Does_Not_Move ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        input.GetKey(KeyCode.UpArrow).Returns(true);
                        arg.MoveNext();
                        input.GetKey(KeyCode.UpArrow).Returns(false);
                        input.GetKey(KeyCode.RightArrow).Returns(true);
                        arg.MoveNext();
                    }
                );

                map[Vector2Int.up * 2].Returns(Tile.Wall);
                map[Vector2Int.up + Vector2Int.right].Returns(Tile.Wall);

                model.Enable();

                Assert.AreEqual(Vector2Int.up, model.Position);
            }

            [Test]
            public void If_Hit_Wall_Does_Not_Raise_OnPositionChange ()
            {
                bool? raised = null;

                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        input.GetKey(KeyCode.UpArrow).Returns(true);
                        arg.MoveNext();
                        model.OnPositionChanged += () => raised = true;
                        input.GetKey(KeyCode.UpArrow).Returns(false);
                        input.GetKey(KeyCode.RightArrow).Returns(true);
                        arg.MoveNext();
                    }
                );

                map[Vector2Int.up * 2].Returns(Tile.Wall);
                map[Vector2Int.up + Vector2Int.right].Returns(Tile.Wall);

                model.Enable();

                Assert.IsNull(raised);
            }

            [Test]
            public void If_Hit_Wall_Does_Not_Change_Direction_And_Continue_Moving_If_Possible ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        input.GetKey(KeyCode.UpArrow).Returns(true);
                        arg.MoveNext();
                        input.GetKey(KeyCode.UpArrow).Returns(false);
                        input.GetKey(KeyCode.RightArrow).Returns(true);
                        arg.MoveNext();
                    }
                );

                map[Vector2Int.up + Vector2Int.right].Returns(Tile.Wall);

                model.Enable();

                Assert.AreEqual(Vector2Int.up * 2, model.Position);
            }

            [Test]
            public void If_Hit_Teleport_Tile_Reposition_To_Other_Teleport ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                input.GetKey(KeyCode.UpArrow).Returns(true);
                map.TeleportPositions.Returns(new[] { Vector2Int.up, Vector2Int.up * -1 });

                map[Vector2Int.up].Returns(Tile.Teleport);

                model.Enable();

                Assert.AreEqual(Vector2Int.up * -1, model.Position);
            }

            [Test]
            public void If_Hit_Teleport_Tile_Dont_Reposition_If_No_Available_Teleports ()
            {
                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                input.GetKey(KeyCode.UpArrow).Returns(true);
                map.TeleportPositions.Returns(new[] { Vector2Int.up });

                map[Vector2Int.up].Returns(Tile.Teleport);

                model.Enable();

                Assert.AreEqual(Vector2Int.up, model.Position);
            }

            [Test]
            public void If_Hit_Teleport_Tile_Raise_On_Teleport ()
            {
                bool? raised = null;
                model.OnTeleport += () => raised = true;

                runner.WhenForAnyArgs(x => x.StartCoroutine(default))
                    .Do(x =>
                    {
                        IEnumerator arg = x.Arg<IEnumerator>();
                        arg.MoveNext();
                    }
                );

                input.GetKey(KeyCode.UpArrow).Returns(true);
                map.TeleportPositions.Returns(new[] { Vector2Int.up, Vector2Int.up * -1 });

                map[Vector2Int.up].Returns(Tile.Teleport);

                model.Enable();

                Assert.IsTrue(raised);
            }
        }

        class Dispose : PlayerModelTests
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
