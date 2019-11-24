using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SnakeServer.Models
{
    public class Snake
    {
        private readonly List<Point> _points;
        private Point lastDeletedPoint;

        [JsonIgnore]
        public Direction Direction { get; private set; }

        [JsonPropertyName("snake")]
        public IEnumerable<Point> Points => _points;

        [JsonIgnore]
        public Point Head => _points.Last();

        public Snake()
        {
            this._points = new List<Point>();
            this.Direction = Direction.Top;
        }

        public Snake(Point point, int length, Direction direction = Direction.Top)
        {
            _points = new List<Point>();

            for (int i = length - 1; i >= 0; i--)
                _points.Add(new Point(point.X, point.Y + i));

            this.Direction = direction;
        }

        public Snake(IEnumerable<Point> points, Direction direction = Direction.Top)
        {
            if (points == null)
                throw new NullReferenceException($"Значение {nameof(points)} должно быть определено");

            _points = new List<Point>(points);
            this.Direction = direction;
        }

        public void Move(Direction newDirection)
        {
            this.lastDeletedPoint = _points.First();
            this._points.Remove(this.lastDeletedPoint);

            this._points.Add(GetNextPoint(newDirection));
            this.Direction = newDirection;
        }

        private Point GetNextPoint(Direction newDirection)
        {
            switch (newDirection)
            {
                case Direction.Top:
                    {
                        if (this.Direction != Direction.Bottom)
                            return new Point(this.Head.X, this.Head.Y - 1);
                        else
                            return new Point(this.Head.X, this.Head.Y + 1);
                    }
                case Direction.Bottom:
                    {
                        if (this.Direction != Direction.Top)
                            return new Point(this.Head.X, this.Head.Y + 1);
                        else
                            return new Point(this.Head.X, this.Head.Y - 1);
                    }
                case Direction.Left:
                    {
                        if (this.Direction != Direction.Right)
                            return new Point(this.Head.X - 1, this.Head.Y);
                        else
                            return new Point(this.Head.X + 1, this.Head.Y);
                    }

                case Direction.Right:
                    {
                        if (this.Direction != Direction.Left)
                            return new Point(this.Head.X + 1, this.Head.Y);
                        else
                            return new Point(this.Head.X - 1, this.Head.Y);
                    }
                default:
                    throw new NotSupportedException($"Данный тип Type({nameof(newDirection)}) = {newDirection} не поддерживается");
            }
        }

        public void Eat()
        {
            this._points.Insert(0, this.lastDeletedPoint);
        }
    }
}
