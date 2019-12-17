using NUnit.Framework;
using SnakeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeServer.Tests.UnitTests
{
    [TestFixture]
    public class SnakeTests
    {
        [Test]
        public void Snake_ShouldCreateCorrectSnakeWithInitLength()
        {
            //Arrange
            Point middlePoint = new Point(0, 0);
            int length = 3;
            Direction direction = Direction.Right;
            List<Point> correctPoints = new List<Point> { new Point(0, 2), new Point(0, 1), new Point(0, 0) };

            //Act
            Snake snake = new Snake(middlePoint, length, direction);

            //Assert
            Assert.AreEqual(length, snake.Points.Count(), "Создана змейка неверного размера");
            Assert.AreEqual(direction, snake.Direction, "Неверное начальное направление");
            for (int i = 0; i < length; i++)
                Assert.AreEqual(correctPoints[i], snake.Points.ElementAt(i), "Добавлена неверная точка в змейку");
        }

        [Test]
        public void Snake_DefaultDirectionShouldBeTop()
        {
            //Arrange and act
            Snake snake1 = new Snake();
            Snake snake2 = new Snake(new List<Point>());
            Snake snake3 = new Snake(new Point(0, 0), 1);

            //Assert
            Assert.AreEqual(Direction.Top, snake1.Direction, "Значение по умолчанию задано неправильно");
            Assert.AreEqual(Direction.Top, snake2.Direction, "Значение по умолчанию задано неправильно");
            Assert.AreEqual(Direction.Top, snake3.Direction, "Значение по умолчанию задано неправильно");
        }

        [Test]
        public void Snake_ShouldCreateCorrectSnake()
        {
            //Arrange
            IEnumerable<Point> correctPoints = GetTestSnakePoints();

            //Act
            Snake snake = new Snake(correctPoints);

            //Assert
            Assert.AreEqual(correctPoints.Count(), snake.Points.Count());

            for (int i = 0; i < correctPoints.Count(); i++)
                Assert.AreEqual(correctPoints.ElementAt(i), snake.Points.ElementAt(i));
        }

        [Test]
        public void Snake_ShouldThrowExceptions()
        {
            //Arrange act and assert
            Assert.Throws<ArgumentNullException>(() => new Snake(null));
            Assert.Throws<ArgumentNullException>(() => new Snake(null, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Snake(new Point(0, 0), 0), "Недопустимо создавать змейку размером меньше 1");
        }

        [TestCaseSource(nameof(DirectionTestCases))]
        public void Move_ShouldMoveSnake((Direction direction, IEnumerable<Point> pointsAfterMove) tuple)
        {
            //Arrange
            Snake snake = new Snake(GetTestSnakePoints());

            //Act
            snake.Move(tuple.direction);

            //Assert
            for (int i = 0; i < tuple.pointsAfterMove.Count(); i++)
                Assert.AreEqual(tuple.pointsAfterMove.ElementAt(i), snake.Points.ElementAt(i), "Змейка двинулась в некорректную точку");
        }

        [Test]
        public void Move_ShouldMoveSnakeTop_WhenDirectionBottom()
        {
            //Arrange
            IEnumerable<Point> pointsAfterMove = new List<Point> { new Point(10, 9), new Point(10, 8), new Point(10, 7) };
            Snake snake = new Snake(GetTestSnakePoints());
            Direction direction = Direction.Bottom;

            //Act
            snake.Move(direction);

            //Assert
            for (int i = 0; i < pointsAfterMove.Count(); i++)
                Assert.AreEqual(pointsAfterMove.ElementAt(i), snake.Points.ElementAt(i), "Змейка должна была двинуться вперед");
        }

        [Test]
        public void Eat_SnakeShouldIncrease()
        {
            //Arrange
            Snake snake = new Snake(GetTestSnakePoints());
            snake.Move(Direction.Top);

            //Act
            snake.Eat();

            //Assert
            IEnumerable<Point> correctPoints = new List<Point> { new Point(10, 10), new Point(10, 9), new Point(10, 8), new Point(10, 7) };
            for (int i = 0; i < correctPoints.Count(); i++)
                Assert.AreEqual(correctPoints.ElementAt(i), snake.Points.ElementAt(i), "Змейка состоит из неверных точек");
        }

        [Test]
        public void Eat_ShouldThrowException_WhenSnakeNotMove()
        {
            //Arrange
            Snake snake = new Snake(GetTestSnakePoints());

            //Act and Assert
            Assert.Throws<NullReferenceException>(() => snake.Eat());
        }

        private static IEnumerable<(Direction direction, IEnumerable<Point> pointsAfterMove)> DirectionTestCases
        {
            get
            {
                yield return (Direction.Top, new List<Point> { new Point(10, 9), new Point(10, 8), new Point(10, 7) });
                yield return (Direction.Right, new List<Point> { new Point(10, 9), new Point(10, 8), new Point(11, 8) });
                yield return (Direction.Left, new List<Point> { new Point(10, 9), new Point(10, 8), new Point(9, 8) });
            }
        }

        private IEnumerable<Point> GetTestSnakePoints()
        {
            return new List<Point>
            {
                new Point(10, 10),
                new Point(10, 9),
                new Point(10, 8)
            };
        }
    }
}
