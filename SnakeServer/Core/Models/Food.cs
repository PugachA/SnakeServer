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
        /// Свойство, инкапсулиющее количество точек  
        /// </summary>
        public IEnumerable<Point> Points => _points;

        public Food()
        {
            this._points = new List<Point>();
        }

        public Food(IEnumerable<Point> points)
        {
            if (points == null)
                throw new ArgumentNullException($"Значение '{nameof(points)}' должно быть определено");

            this._points = new List<Point>(points);
        }

        /// <summary>
        /// Генерация новой точки на доске, не пересекающая с переданными точками
        /// </summary>
        /// <param name="snakePoints">Точки, с которыми не должна пересекаться новая точка</param>
        /// <param name="boardSize">Размер доски</param>
        public void GenerateFood(IEnumerable<Point> snakePoints, Size boardSize)
        {
            Point newFood;

            Random random = new Random();
            List<Point> allPoints = GetAllBoardPoints(boardSize);

            do
            {
                if (!allPoints.Any())
                {
                    newFood = null;
                    break;
                }

                int index = random.Next(0, allPoints.Count);
                newFood = allPoints[index];
                allPoints.RemoveAt(index);
            } while (snakePoints.Contains(newFood) || this._points.Contains(newFood)); //точка не должна пересекаться с змейкой и старыми точками 

            if (newFood == null)
                throw new NullReferenceException("Не удалось сгенерировать новую точку для еды");

            _points.Add(newFood);
        }

        /// <summary>
        /// Генерирует все точки поля
        /// </summary>
        /// <param name="boardSize">Размеры поля</param>
        /// <returns>Все точки поля</returns>
        private List<Point> GetAllBoardPoints(Size boardSize)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < boardSize.Height; i++)
                for (int j = 0; j < boardSize.Width; j++)
                    points.Add(new Point(j, i));

            return points;
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
