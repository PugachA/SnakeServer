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

        public Snake()
        {
            _points = new List<Point>();
            direction = Direction.Top;
        }

        public Snake(Point point, int length, Direction direction = Direction.Top)
        {
            if (point == null)
                throw new NullReferenceException($"Значение {nameof(point)} должно быть определено");

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
            switch (newDirection)
            {
                case Direction.Top:
                    {
                        if (direction != Direction.Bottom)
                        {
                            this._points.RemoveAt(0);

                            Point head = _points.Last();
                            Point newHead = new Point(head.X, head.Y - 1);
                            this._points.Add(newHead);

                            this.direction = newDirection;
                        }
                        break;
                    }
                case Direction.Bottom:
                    {
                        if (direction != Direction.Top)
                        {
                            _points.RemoveAt(0);

                            Point head = _points.Last();
                            Point newHead = new Point(head.X, head.Y + 1);
                            _points.Add(newHead);

                            direction = newDirection;
                        }
                        break;
                    }
                case Direction.Left:
                    {
                        if (direction != Direction.Right)
                        {
                            _points.RemoveAt(0);

                            Point head = _points.Last();
                            Point newHead = new Point(head.X - 1, head.Y);
                            _points.Add(newHead);

                            direction = newDirection;
                        }
                        break;
                    }

                case Direction.Right:
                    {
                        if (direction != Direction.Left)
                        {
                            _points.RemoveAt(0);

                            Point head = _points.Last();
                            Point newHead = new Point(head.X + 1, head.Y);
                            _points.Add(newHead);

                            direction = newDirection;
                        }
                        break;
                    }

            }

        }

        private void GetNextPoint(Direction newDirection)
        {

        }
    }
}
