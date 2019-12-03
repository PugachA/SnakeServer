using NUnit.Framework;
using SnakeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeServer.Tests.UnitTests
{
    [TestFixture]
    public class FoodTests
    {
        private Size size;

        [SetUp]
        public void Setup()
        {
            this.size = new Size { Height = 20, Width = 20 };
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
        public void GenerateFood_PointShouldNotBeOutsideBoard()
        {
            //Arrange
            List<Point> snakePoints = new List<Point>();
            Food food = new Food();

            //Act
            for (int i = 0; i < this.size.Height * this.size.Width; i++)
                food.GenerateFood(snakePoints, this.size);

            //Assert
            var outsideBoardPoints = food.Points.Where(p => p.X < 0 || p.X > this.size.Width || p.Y < 0 || p.Y > this.size.Height);
            Assert.IsFalse(outsideBoardPoints.Any(), "Найдены точки за границой игрового поля");
            Assert.AreEqual(this.size.Height * this.size.Width, food.Points.Count(), "Сгенерировано неверное количество точек");
        }

        [Test]
        public void GenerateFood_ShouldCreateDistinctPoints()
        {
            //Arrange
            List<Point> snakePoints = new List<Point>();
            Food food = new Food();

            //Act
            for (int i = 0; i < this.size.Height * this.size.Width; i++)
                food.GenerateFood(snakePoints, this.size);

            //Assert
            var distinct = food.Points.Distinct();
            var noDistinct = food.Points.GroupBy(p => p).Where(grp => grp.Count() > 1);
            Assert.AreEqual(food.Points.Count(), food.Points.Distinct().Count(), "Найдены повторяющие точки");
        }

        [Test]
        public void GenerateFood_RandomTest()
        {
            //Arrange
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

    }
}
