using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeServer.Models
{
    public class Food
    {
        private readonly List<Point> _points;
        private readonly Random random;

        [JsonPropertyName("food")]
        public IEnumerable<Point> Points => _points;


        public Food()
        {
            this._points = new List<Point>();
            this.random = new Random();
        }

        public Food(IEnumerable<Point> points)
        {
            if(points == null)
                throw new NullReferenceException($"Значение {nameof(points)} должно быть определено");

            this._points = new List<Point>(points);
            this.random = new Random();
        }

        public void GenerateFood(IEnumerable<Point> snakePoints, Size boardSize)
        {
            int count = 0;
            Point newFood = new Point();

            do
            {
                newFood = new Point(this.random.Next(0, boardSize.Width - 1), this.random.Next(0, boardSize.Heigth - 1));
                count++;
            } while (snakePoints.Contains(newFood) || count > boardSize.Heigth * boardSize.Width);

            _points.Add(newFood);
        }

    }
}
