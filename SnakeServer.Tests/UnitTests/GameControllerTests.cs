using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NLog.Extensions.Logging;
using NUnit.Framework;
using SnakeServer.Controllers;
using SnakeServer.Core.Interfaces;
using SnakeServer.Core.Models;
using SnakeServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeServer.Tests.UnitTests
{
    [TestFixture(Category = "Unit")]
    public class GameControllerTests
    {
        private ServiceProvider serviceProvider;
        private Mock<IGameManager> mockGame;
        private Mock<IGameService> mockService;
        private Mock<ILogger<GameController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            //TODO: Transfer to integration tests
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddNLog("nlog.config"));
            services.AddSingleton<IGameService, GameManagerService>();
            services.AddTransient<GameController>();
            this.serviceProvider = services.BuildServiceProvider();

            // For unit tests
            this.mockGame = new Mock<IGameManager>();
            mockGame.Setup(g => g.GetGameBoard()).Returns(GetTestGameBoard());
            mockGame.Setup(g => g.GetSnake()).Returns(GetTestSnake());
            mockGame.Setup(g => g.GetFood()).Returns(GetTestFood());

            this.mockService = new Mock<IGameService>();
            mockService.Setup(service => service.Game).Returns(this.mockGame.Object);

            this.mockLogger = new Mock<ILogger<GameController>>();
        }

        [TestCaseSource(nameof(FuncTestCases))]
        public void GetMethods_ReturnsOkWithData((Type type, Func<GameController, object> getResult) tuple)
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);

            //Act
            var result = tuple.getResult(controller);
            OkObjectResult okObjectResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okObjectResult);
            Assert.IsInstanceOf(tuple.type, okObjectResult.Value);
        }

        [TestCaseSource(nameof(FuncTestCases))]
        public void GetMethods_Returns500StatusCode_WhenException((Type type, Func<GameController, object> getResult) tuple)
        {
            //Arrange
            var controller = new GameController(null, this.mockLogger.Object);

            //Act
            var result = tuple.getResult(controller);
            ObjectResult objectResult = result as ObjectResult;

            //Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [Test]
        public void GetGameBoard_ReturnsCorrectData()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);

            //Act
            var result = controller.GetGameboard();
            var gameBoard = GetTestGameBoard();

            //Assert
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            GameBoard value = okObjectResult.Value as GameBoard;
            Assert.NotNull(value);

            Assert.AreEqual(gameBoard.TurnNumber, value.TurnNumber);
            Assert.AreEqual(gameBoard.InitialSnakeLength, value.InitialSnakeLength);
            Assert.AreEqual(gameBoard.TimeUntilNextTurnMilliseconds, value.TimeUntilNextTurnMilliseconds);
            Assert.AreEqual(gameBoard.GameBoardSize.Height, value.GameBoardSize.Height);
            Assert.AreEqual(gameBoard.GameBoardSize.Width, value.GameBoardSize.Width);
        }

        [Test]
        public void GetSnake_ReturnsCorrectData()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);

            //Act
            var result = controller.GetSnake();
            var snake = GetTestSnake();

            //Assert
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            Snake value = okObjectResult.Value as Snake;
            Assert.NotNull(value);

            Assert.AreEqual(snake.Points.Count(), value.Points.Count());

            for (int i = 0; i < snake.Points.Count(); i++)
                Assert.AreEqual(snake.Points.ElementAt(i), value.Points.ElementAt(i));

            Assert.AreEqual(snake.Head, value.Head);
            Assert.AreEqual(snake.Direction, value.Direction);
        }

        [Test]
        public void GetFood_ReturnsCorrectData()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);

            //Act
            var result = controller.GetFood();
            var food = GetTestFood();

            //Assert
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            Food value = okObjectResult.Value as Food;
            Assert.NotNull(value);

            Assert.AreEqual(food.Points.Count(), value.Points.Count());

            for (int i = 0; i < food.Points.Count(); i++)
                Assert.AreEqual(food.Points.ElementAt(i), value.Points.ElementAt(i));
        }

        [Test]
        public void PostDirection_ReturnsBadRequest_WhenModelInvalid()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);
            controller.ModelState.AddModelError("Direction", "Required");
            DirectionObject directionObject = new DirectionObject();

            //Act
            var result = controller.PostDirection(directionObject);
            var badRequestObject = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(badRequestObject);
        }

        [Test]
        public void PostDirection_ReturnsOkResult_WhenModelValid()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);
            DirectionObject directionObject = new DirectionObject();

            //Act
            var result = controller.PostDirection(directionObject);
            var okObject = result as OkResult;

            //Assert
            Assert.NotNull(okObject);
        }

        public void PostDirection_Returns500StatusCode_WhenException()
        {
            //Arrange
            var controller = new GameController(null, null);
            DirectionObject directionObject = new DirectionObject();

            //Act
            var result = controller.PostDirection(directionObject);
            var okObject = result as ObjectResult;

            //Assert
            Assert.NotNull(okObject);
            Assert.AreEqual(500, okObject.StatusCode);
        }

        [Test]
        public void PostDirection_UpdateDirection()
        {
            //Arrange
            mockGame.Setup(g => g.UpdateDirection(Direction.Right)).Verifiable();
            var controller = new GameController(mockService.Object, mockLogger.Object);
            DirectionObject directionObject = new DirectionObject { Direction = Direction.Right};

            //Act
            var result = controller.PostDirection(directionObject);
            var okObject = result as OkResult;

            //Assert
            Assert.NotNull(okObject);
            mockGame.Verify();
        }

        private static IEnumerable<(Type type, Func<GameController, object> getResult)> FuncTestCases
        {
            get
            {
                yield return (typeof(GameBoard), c => c.GetGameboard());
                yield return (typeof(Snake), c => c.GetSnake());
                yield return (typeof(Food), c => c.GetFood());
            }
        }

        private GameBoard GetTestGameBoard()
        {
            return new GameBoard
            {
                TurnNumber = 0,
                InitialSnakeLength = 2,
                TimeUntilNextTurnMilliseconds = 600,
                GameBoardSize = new Size { Height = 20, Width = 20 }
            };
        }

        private Snake GetTestSnake()
        {
            List<Point> snake = new List<Point> { new Point(0, 0), new Point(1, 1) };
            return new Snake(snake);
        }

        private Food GetTestFood()
        {
            List<Point> food = new List<Point> { new Point(0, 0) };
            return new Food(food);
        }
    }
}