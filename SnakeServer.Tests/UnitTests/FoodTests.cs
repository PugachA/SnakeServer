using NUnit.Framework;
using SnakeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeServer.Tests.UnitTests
{
    [TestFixture]
    public class FoodTests
    {
        private Size size;
        private Food food;

        [SetUp]
        public void Setup()
        {
            this.size = new Size { Height = 20, Width = 20 };
            this.food = new Food();
        }

        [Test]
        public void Food_ShouldThrowNullReferenceException()
        {
            //Arrange
            IEnumerable<Point> points = null;

            //Act and Assert
            Assert.Throws<NullReferenceException>(() => new Food(points));
        }

        [Test]
        public void Food_CreateCorrectPoints()
        {
            //Arrange
            IEnumerable<Point> points = GetTestFoodPoints();

            //Act
            Food food = new Food(points);

            //Assert
            Assert.AreEqual(points.Count(), food.Points.Count());

            for (int i = 0; i < points.Count(); i++)
                Assert.AreEqual(points.ElementAt(i), food.Points.ElementAt(i));
        }

        [Test]
        public void GenerateFood_PointsShouldNotBeOutsideBoard()
        {
            //Arrange
            List<Point> snakePoints = new List<Point>();

            //Act
            for (int i = 0; i < size.Height * size.Width; i++)
                food.GenerateFood(snakePoints, size);

            //Assert
            var outsideBoardPoints = food.Points.Where(p => p.X < 0 || p.Y < 0 || p.X > size.Width - 1 || p.Y > size.Height - 1);
            Assert.IsFalse(outsideBoardPoints.Any(), "Найдены точки за границой игрового поля");
            Assert.AreEqual(size.Height * size.Width, food.Points.Count(), "Сгенерировано неверное количество точек");
        }

        [Test]
        public void GenerateFood_ShouldCreateDistinctPoints()
        {
            //Arrange
            List<Point> snakePoints = new List<Point>();

            //Act
            for (int i = 0; i < size.Height * size.Width; i++)
                food.GenerateFood(snakePoints, size);

            //Assert
            Assert.AreEqual(food.Points.Count(), food.Points.Distinct().Count(), "Найдены повторяющие точки");
        }

        [Test]
        public void GenerateFood_ThrowNullReferencesException()
        {
            //Arrange
            List<Point> snakePoints = new List<Point>();
            Size size = new Size { Height = 1, Width = 1 };

            //Act
            food.GenerateFood(snakePoints, size);

            //Assert
            Assert.Throws<NullReferenceException>(() => food.GenerateFood(snakePoints, size));
        }

        [Test]
        public void GenerateFood_ShouldNotIntersectWithSnake()
        {
            //Arrange
            IEnumerable<Point> snakePoints = GetTestSnakePoints();

            //Act
            for (int i = 0; i < size.Height * size.Width - snakePoints.Count(); i++)
                food.GenerateFood(snakePoints, size);

            //Assert
            Assert.IsFalse(snakePoints.Intersect(food.Points).Any(), "Найдены точки, пересекающиеся со змейкой");
        }

        [Test]
        public void DeleteFood_ShouldDeleteCorrectPoint()
        {
            //Arrange
            IEnumerable<Point> points = GetTestFoodPoints();
            Food food = new Food(points);
            Point pointForDelete = points.ElementAt(0);

            //Act
            food.DeleteFood(pointForDelete);

            //Assert
            Assert.AreEqual(points.Count() - 1, food.Points.Count(), "Не произошло удаление точки");
            Assert.IsFalse(food.Points.Contains(pointForDelete), "Не удалена требуемая точка");
        }

        private IEnumerable<Point> GetTestFoodPoints()
        {
            Random random = new Random();
            return new List<Point>
            {
                new Point(random.Next(0, 1000), random.Next(0, 1000)),
                new Point(random.Next(0, 1000), random.Next(0, 1000)),
                new Point(random.Next(0, 1000), random.Next(0, 1000))
            };
        }

        private IEnumerable<Point> GetTestSnakePoints()
        {
            Random random = new Random();

            return new List<Point>
            {
                new Point(random.Next(0, size.Width), random.Next(0, size.Height)),
                new Point(random.Next(0, size.Width), random.Next(0, size.Height)),
                new Point(random.Next(0, size.Width), random.Next(0, size.Height)),
                new Point(random.Next(0, size.Width), random.Next(0, size.Height)),
                new Point(random.Next(0, size.Width), random.Next(0, size.Height)),
                new Point(random.Next(0, size.Width), random.Next(0, size.Height))
            };
        }

    }
}
