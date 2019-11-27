using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SnakeServer.Core.Models
{
    public class Food
    {
        /// <summary>
        /// Точки, где располагается еда на доске
        /// </summary>
        private readonly List<Point> _points;
        
        /// <summary>
        /// Объект для генерации случайным образом точек 
        /// </summary>
        private readonly Random random;

        /// <summary>
        /// Свойство, инкапсулиющее количество точек  
        /// </summary>
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
                throw new NullReferenceException($"Значение '{nameof(points)}' должно быть определено");

            this._points = new List<Point>(points);
            this.random = new Random();
        }

        /// <summary>
        /// Генерация новой точки на доске, не пересекающая с переданными точками
        /// </summary>
        /// <param name="snakePoints">Точки, с которыми не должна пересекаться новая точка</param>
        /// <param name="boardSize">Размер доски</param>
        public void GenerateFood(IEnumerable<Point> snakePoints, Size boardSize)
        {
            int count = 0;
            Point newFood;

            do
            {
                newFood = new Point(this.random.Next(0, boardSize.Width - 1), this.random.Next(0, boardSize.Height - 1));
                count++;
            } while (snakePoints.Contains(newFood) || count > boardSize.Height * boardSize.Width);

            if (newFood == null)
                throw new AggregateException("Не удалось сгенерировать новую точку для еды");

            _points.Add(newFood);
        }

        /// <summary>
        /// Удаление переданноу точки
        /// </summary>
        /// <param name="point">Точка для удаления</param>
        public void DeleteFood(Point point)
        {
            if (point == null)
                throw new NullReferenceException($"Значение '{nameof(point)}' должно быть определено");

            _points.Remove(point);
        }
    }
}
