using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NLog.Extensions.Logging;
using NUnit.Framework;
using SnakeServer.Controllers;
using SnakeServer.Core.Interfaces;
using SnakeServer.Core.Models;
using SnakeServer.DTO;
using SnakeServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeServer.Tests.UnitTests
{
    [TestFixture(Category = "Unit")]
    public class GameControllerTests
    {
        private Mock<IGameManager> mockGame;
        private Mock<IGameService> mockService;
        private Mock<ILogger<GameController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            this.mockGame = new Mock<IGameManager>();
            mockGame.Setup(g => g.GameBoardSettings).Returns(TestGameBoardSettings);
            mockGame.Setup(g => g.Snake).Returns(GetTestSnake());
            mockGame.Setup(g => g.Food).Returns(GetTestFood());
            mockGame.Setup(g => g.TurnNumber).Returns(0);

            this.mockService = new Mock<IGameService>();
            mockService.Setup(service => service.Game).Returns(this.mockGame.Object);

            this.mockLogger = new Mock<ILogger<GameController>>();
        }

        [Test]
        public void GetGameBoard_ReturnsOkWithData()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);

            //Act
            var result = controller.GetGameboard();
            OkObjectResult okObjectResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okObjectResult, "Контроллер возвращает некорректный результат");
            Assert.IsInstanceOf<GameBoardDto>(okObjectResult.Value, "Контроллер возвращает некорректный тип");
        }

        [Test]
        public void GetGameBoard_Returns500StatusCode_WhenException()
        {
            //Arrange
            var controller = new GameController(null, this.mockLogger.Object);

            //Act
            var result = controller.GetGameboard();
            ObjectResult objectResult = result as ObjectResult;

            //Assert
            Assert.NotNull(objectResult, "Контроллер возвращает некорректный результат");
            Assert.AreEqual(500, objectResult.StatusCode, "Контроллер возвращает некорректный код ошибки");
        }

        [TestCaseSource(nameof(FuncTestCases))]
        public void GetGameBoard_ReturnCorrectPoints((string name, Func<GameBoardDto, IEnumerable<Point>> getPoints) tuple)
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);

            //Act
            var result = controller.GetGameboard();
            GameBoardDto correctDto = TestGameBoardDto();

            //Assert
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult, "Контроллер возвращает некорректный результат");

            GameBoardDto gameBoard = okObjectResult.Value as GameBoardDto;
            Assert.NotNull(gameBoard, "Контроллер возвращает некорректный результат");

            Assert.AreEqual(tuple.getPoints(correctDto).Count(), tuple.getPoints(gameBoard).Count());

            for (int i = 0; i < tuple.getPoints(correctDto).Count(); i++)
                Assert.AreEqual(tuple.getPoints(correctDto).ElementAt(i), tuple.getPoints(gameBoard).ElementAt(i));
        }

        [Test]
        public void GetGameBoard_ReturnCorrectGameBoardSettings()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);

            //Act
            var result = controller.GetGameboard();
            GameBoardDto correctDto = TestGameBoardDto();

            //Assert
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult, "Контроллер возвращает некорректный результат");

            GameBoardDto gameBoard = okObjectResult.Value as GameBoardDto;
            Assert.NotNull(gameBoard, "Контроллер возвращает некорректный результат");

            Assert.AreEqual(correctDto.TurnNumber, gameBoard.TurnNumber);
            Assert.AreEqual(correctDto.TimeUntilNextTurnMilliseconds, gameBoard.TimeUntilNextTurnMilliseconds);
            Assert.AreEqual(correctDto.GameBoardSize.Height, gameBoard.GameBoardSize.Height);
            Assert.AreEqual(correctDto.GameBoardSize.Width, gameBoard.GameBoardSize.Width);
        }

        [Test]
        public void PostDirection_ReturnsOkResult_WhenModelValid()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);
            DirectionDto directionObject = new DirectionDto();

            //Act
            var result = controller.PostDirection(directionObject);
            var okObject = result as OkResult;

            //Assert
            Assert.NotNull(okObject, "Контроллер возвращает некорректный результат");
        }

        [Test]
        public void PostDirection_ReturnsBadRequest_WhenModelInvalid()
        {
            //Arrange
            var controller = new GameController(this.mockService.Object, this.mockLogger.Object);
            controller.ModelState.AddModelError("Direction", "Required");
            DirectionDto directionObject = new DirectionDto();

            //Act
            var result = controller.PostDirection(directionObject);
            var badRequestObject = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(badRequestObject, "Контроллер возвращает некорректный результат");
        }

        [Test]
        public void PostDirection_Returns500StatusCode_WhenException()
        {
            //Arrange
            var controller = new GameController(null, this.mockLogger.Object);
            DirectionDto directionObject = new DirectionDto();

            //Act
            var result = controller.PostDirection(directionObject);
            var okObject = result as ObjectResult;

            //Assert
            Assert.NotNull(okObject, "Контроллер возвращает некорректный результат");
            Assert.AreEqual(500, okObject.StatusCode, "Контроллер возвращает некорректный код ошибки");
        }

        [Test]
        public void PostDirection_UpdateDirection()
        {
            //Arrange
            mockGame.Setup(g => g.UpdateDirection(Direction.Right)).Verifiable();
            var controller = new GameController(mockService.Object, mockLogger.Object);
            DirectionDto directionObject = new DirectionDto { Direction = Direction.Right };

            //Act
            var result = controller.PostDirection(directionObject);
            var okObject = result as OkResult;

            //Assert
            Assert.NotNull(okObject, "Контроллер возвращает некорректный результат");
            Assert.DoesNotThrow(() => mockGame.Verify());
        }

        private static IEnumerable<(string, Func<GameBoardDto, IEnumerable<Point>>)> FuncTestCases
        {
            get
            {
                yield return ("Snake", c => c.Snake );
                yield return ("Food", c => c.Food);
            }
        }

        private GameBoardSettings TestGameBoardSettings
        {
            get
            {
                return new GameBoardSettings
                {
                    InitialSnakeLength = 2,
                    TimeUntilNextTurnMilliseconds = 600,
                    GameBoardSize = new Size { Height = 20, Width = 20 }
                };
            }
        }

        private IEnumerable<Point> GetTestSnake() => new List<Point> { new Point(0, 0), new Point(1, 1) };

        private IEnumerable<Point> GetTestFood() => new List<Point> { new Point(0, 0) };

        private GameBoardDto TestGameBoardDto()
        {
            return new GameBoardDto
            {
                TurnNumber = 0,
                TimeUntilNextTurnMilliseconds = TestGameBoardSettings.TimeUntilNextTurnMilliseconds,
                GameBoardSize = TestGameBoardSettings.GameBoardSize,
                Food = GetTestFood(),
                Snake = GetTestSnake()
            };
        }
    }
}