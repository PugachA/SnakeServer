using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeServer.Models
{
    public class Snake : IGameBoardItem
    {
        private readonly List<Point> _points;

        [JsonPropertyName("snake")]
        public IEnumerable<Point> Points => _points;

        public Snake()
        {
            _points = new List<Point>();
        }
        public Point this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Add(Point point)
        {
            _points.Add(point);
        }
    }
}
