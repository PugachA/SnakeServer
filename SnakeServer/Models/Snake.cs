using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SnakeServer.Models
{
    public class Snake
    {
        private readonly List<Point> _points;

        private Direction direction;

        [JsonPropertyName("snake")]
        public IEnumerable<Point> Points => _points;

        [JsonIgnore]
        public Point Head => _points.Last();  

        public Snake()
        {
            this._points = new List<Point>();
            this.direction = Direction.Top;
        }

        public Snake(Point point, int length, Direction direction = Direction.Top)
        {
            _points = new List<Point>();

            for (int i = length - 1; i >= 0; i--)
                _points.Add(new Point(point.X, point.Y + i));

            this.direction = direction;
 
        }

        public Snake(IEnumerable<Point> points, Direction direction = Direction.Top)
        {
            if (points == null)
                throw new NullReferenceException($"Значение {nameof(points)} должно быть определено");

            _points = new List<Point>(points);
            this.direction = direction;
        }

        public void Move(Direction newDirection)
        {
            this._points.RemoveAt(0);
            this._points.Add(GetNextPoint(newDirection));
            this.direction = newDirection;
        }

        private Point GetNextPoint(Direction newDirection)
        {
            switch (newDirection)
            {
                case Direction.Top:
                    {
                        if (direction != Direction.Bottom)
                            return new Point(this.Head.X, this.Head.Y - 1);
                        break;
                    }
                case Direction.Bottom:
                    {
                        if (direction != Direction.Top)
                            return new Point(this.Head.X, this.Head.Y + 1);
                        break;
                    }
                case Direction.Left:
                    {
                        if (direction != Direction.Right)
                           return new Point(this.Head.X - 1, this.Head.Y);
                        break;
                    }

                case Direction.Right:
                    {
                        if (direction != Direction.Left)
                            return new Point(this.Head.X + 1, this.Head.Y);
                        break;
                    }
                default:
                        throw new NotSupportedException($"Данный тип Type({nameof(newDirection)}) = {newDirection} не поддерживается");
            }

            return null;
        }
    }
}
