using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SnakeServer.Core.Models;
using SnakeServer.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeServer.Tests.UnitTests
{
    [TestFixture]
    public class GameManagerTest
    {
        private Mock<ILogger> mockLogger;
        private GameBoardSettings gameBoardSettings;

        [SetUp]
        public void Setup()
        {
            this.mockLogger = new Mock<ILogger>();
            gameBoardSettings = new GameBoardSettings
            {
                GameBoardSize = new Size { Height = 20, Width = 20 },
                InitialSnakeLength = 2,
                TimeUntilNextTurnMilliseconds = 600,
            };
        }

        [Test]
        public void GameManager_ShouldThrowsException()
        {
            //Arrange
            Food food = new Food();
            Snake snake = new Snake();
            Direction direction = Direction.Top;

            //Act and assert
            Assert.Throws<ArgumentNullException>(() => new GameManager(null, food, this.gameBoardSettings, direction, this.mockLogger.Object));
            Assert.Throws<ArgumentNullException>(() => new GameManager(snake, null, this.gameBoardSettings, direction, this.mockLogger.Object));
            Assert.Throws<ArgumentNullException>(() => new GameManager(snake, food, null, direction, this.mockLogger.Object));
            Assert.Throws<ArgumentNullException>(() => new GameManager(snake, food, this.gameBoardSettings, direction, null));
        }

        [Test]
        public void NextTurn_ShouldMoveSnake()
        {
            //Arrange
            Mock<ISnake> mockSnake = new Mock<ISnake>();
            mockSnake.Setup(s => s.Move(Direction.Top)).Verifiable();

            Food food = new Food();
            Direction direction = Direction.Top;
            GameManager gameManager = new GameManager(mockSnake.Object, food, this.gameBoardSettings, direction, this.mockLogger.Object);

            //Act
            gameManager.NextTurn();

            //Assert
            Assert.DoesNotThrow(() => mockSnake.Verify(), $"Не выполнился метод Move");
            Assert.AreEqual(1, gameManager.TurnNumber, "Не увеличился счетчик шагов");
            Assert.AreEqual(false, gameManager.IsGameOver, "Игра не должна была завершиться");
        }

        [TestCaseSource(nameof(HeadPointTestCases))]
        public void NextTurn_ShouldGameOver_WhenSnakeCrashIntoWall(Point headPoint)
        {
            //Arrange
            Mock<ISnake> mockSnake = new Mock<ISnake>();
            mockSnake.Setup(s => s.Head).Returns(headPoint);

            Food food = new Food();
            Direction direction = Direction.Top;
            GameManager gameManager = new GameManager(mockSnake.Object, food, this.gameBoardSettings, direction, this.mockLogger.Object);

            //Act
            gameManager.NextTurn();

            //Assert
            Assert.IsTrue(gameManager.IsGameOver, "После врезания в стенку игра не завершилась");
        }

        [Test]
        public void NextTurn_ShouldGameOver_WhenSnakeCrashIntoItSelf()
        {
            //Arrange
            IEnumerable<Point> points = new List<Point> { new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(0, 0) };
            Snake snake = new Snake(points);

            Food food = new Food();
            Direction direction = Direction.Top;
            GameManager gameManager = new GameManager(snake, food, this.gameBoardSettings, direction, this.mockLogger.Object);

            //Act
            gameManager.NextTurn();

            //Assert
            Assert.IsTrue(gameManager.IsGameOver, "После поедания себя игра не завершилась");
        }

        [Test]
        public void NextTurn_SnakeShouldEat()
        {
            //Arrange
            Point point = new Point(0, 0);
            Mock<ISnake> mockSnake = new Mock<ISnake>();
            mockSnake.Setup(s => s.Eat()).Verifiable();
            mockSnake.Setup(s => s.Head).Returns(point);

            Food food = new Food(new List<Point> { point });
            Direction direction = Direction.Top;
            GameManager gameManager = new GameManager(mockSnake.Object, food, this.gameBoardSettings, direction, this.mockLogger.Object);

            //Act
            gameManager.NextTurn();

            //Assert
            Assert.DoesNotThrow(() => mockSnake.Verify());
        }

        [Test]
        public void GameBoardSettings_ShouldReturnCorrectSettings()
        {
            //Arrange
            GameManager gameManager = new GameManager(
                new Snake(),
                new Food(),
                this.gameBoardSettings,
                Direction.Top,
                this.mockLogger.Object);

            //Act 
            GameBoardSettings resultBoard = gameManager.GameBoardSettings;

            //Assert
            Assert.AreEqual(this.gameBoardSettings.GameBoardSize.Height, resultBoard.GameBoardSize.Height);
            Assert.AreEqual(this.gameBoardSettings.GameBoardSize.Width, resultBoard.GameBoardSize.Width);
            Assert.AreEqual(this.gameBoardSettings.InitialSnakeLength, resultBoard.InitialSnakeLength);
            Assert.AreEqual(this.gameBoardSettings.TimeUntilNextTurnMilliseconds, resultBoard.TimeUntilNextTurnMilliseconds);
        }

        [Test]
        public void Snake_ShouldReturnCorrectPoints()
        {
            //Arrange
            Snake snake = new Snake(new List<Point> { new Point(0, 0), new Point(0, 1) });
            GameManager gameManager = new GameManager(
                snake,
                new Food(),
                new GameBoardSettings(),
                Direction.Top,
                this.mockLogger.Object);

            //Act 
            IEnumerable<Point> resultSnake = gameManager.Snake;

            //Assert
            Assert.AreEqual(snake.Points.Count(), resultSnake.Count());

            for (int i = 0; i < snake.Points.Count(); i++)
                Assert.AreEqual(snake.Points.ElementAt(i), resultSnake.ElementAt(i));
        }

        [Test]
        public void Food_ShouldReturnCorrectPoints()
        {
            //Arrange
            Food food = new Food(new List<Point> { new Point(0, 0), new Point(0, 1) });
            GameManager gameManager = new GameManager(
                new Snake(),
                food,
                new GameBoardSettings(),
                Direction.Top,
                this.mockLogger.Object);

            //Act 
            IEnumerable<Point> resultFood = gameManager.Food;

            //Assert
            Assert.AreEqual(food.Points.Count(), resultFood.Count());

            for (int i = 0; i < food.Points.Count(); i++)
                Assert.AreEqual(food.Points.ElementAt(i), resultFood.ElementAt(i));
        }


        private static IEnumerable<Point> HeadPointTestCases
        {
            get
            {
                List<Point> result = new List<Point>();
                int heigth = 20;
                int width = 20;

                for (int i = 0; i < heigth; i++)
                {
                    result.Add(new Point(-1, i));
                    result.Add(new Point(width, i));
                }

                for (int i = 0; i < width; i++)
                {
                    result.Add(new Point(i, -1));
                    result.Add(new Point(i, heigth));
                }

                return result;
            }
        }
    }
}
