using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeServer
{
    public class Food : IGameBoardItem
    {
        private readonly List<Point> _points;
        
        [JsonPropertyName("food")]
        public IEnumerable<Point> Points => _points;

        public Food()
        {
            _points = new List<Point>();
        }

        public Point this[int index] => _points[index];

        public void Add(Point point)
        {
            _points.Add(point);
        }

        public void DeleteFirst()
        {
            if (_points.Count > 0)
                _points.RemoveAt(0);
            else
                throw new IndexOutOfRangeException($"Обьект не заполнено данными. Нет возможности удалить элемент");
        }
    }
}
