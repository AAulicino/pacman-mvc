using System;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

public class GameSessionModelTests : MonoBehaviour
{
    GameSessionModel model;
    IGameModelFactory gameModelFactory;
    IMapFactory mapFactory;

    [SetUp]
    public void Setup ()
    {
        gameModelFactory = Substitute.For<IGameModelFactory>();
        mapFactory = Substitute.For<IMapFactory>();

        model = new GameSessionModel(gameModelFactory, mapFactory);
    }

    [Test]
    public void StartNewGame_Raises_OnGame_Start ()
    {
        gameModelFactory.Create(default).ReturnsForAnyArgs(Substitute.For<IGameModel>());

        bool called = false;
        model.OnGameStart += () => called = true;

        model.StartNewGame();

        Assert.IsTrue(called);
    }

    [Test]
    public void Raises_OnGameEnded_True ()
    {
        IGameModel gameModel = Substitute.For<IGameModel>();
        gameModelFactory.Create(default).ReturnsForAnyArgs(gameModel);

        bool? victory = null;
        model.OnGameEnded += x => victory = x;

        model.StartNewGame();

        gameModel.OnGameEnded += Raise.Event<Action<bool>>(true);

        Assert.IsTrue(victory);
    }

    [Test]
    public void Raises_OnGameEnded_False ()
    {
        IGameModel gameModel = Substitute.For<IGameModel>();
        gameModelFactory.Create(default).ReturnsForAnyArgs(gameModel);

        bool? victory = null;
        model.OnGameEnded += x => victory = x;

        model.StartNewGame();

        gameModel.OnGameEnded += Raise.Event<Action<bool>>(true);

        Assert.IsTrue(victory);
    }

    [Test]
    public void StartNewGame_Calls_GameModel_Initialize ()
    {
        IGameModel gameModel = Substitute.For<IGameModel>();
        gameModelFactory.Create(default).ReturnsForAnyArgs(gameModel);

        bool? victory = null;
        model.OnGameEnded += x => victory = x;

        model.StartNewGame();

        gameModel.Received().Initialize();
    }

    [Test]
    public void Sets_GameModel ()
    {
        IGameModel gameModel = Substitute.For<IGameModel>();
        gameModelFactory.Create(default).ReturnsForAnyArgs(gameModel);

        bool? victory = null;
        model.OnGameEnded += x => victory = x;

        model.StartNewGame();

        Assert.AreEqual(gameModel, model.GameModel);
    }
}
