using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SnakeServer.Core.Models
{
    public class Snake
    {
        /// <summary>
        /// Точки из которых состоит змейка
        /// </summary>
        private readonly List<Point> _points;

        /// <summary>
        /// Последняя удаленная точка
        /// </summary>
        private Point lastDeletedPoint;

        /// <summary>
        /// Текущее направление змейки
        /// </summary>
        [JsonIgnore]
        public Direction Direction { get; private set; }

        /// <summary>
        /// Свойство для инкапсуляции точек змейки
        /// </summary>
        [JsonPropertyName("snake")]
        public IEnumerable<Point> Points => _points;

        /// <summary>
        /// Точка соответствующая голове змейки
        /// </summary>
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

            //TODO: Проверить length
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

        /// <summary>
        /// Совершение шага змейкой
        /// </summary>
        /// <param name="newDirection">Новое направление движения</param>
        public void Move(Direction newDirection)
        {
            //Сохраняем удаляемую точку
            this.lastDeletedPoint = _points.First();
            this._points.Remove(this.lastDeletedPoint);

            //Добавление новой точки к голове
            this._points.Add(GetNextPoint(newDirection));
            this.Direction = newDirection;
        }

        /// <summary>
        /// Генерация следующей точки змейки в зависимости от направления
        /// </summary>
        /// <param name="newDirection"></param>
        /// <returns></returns>
        private Point GetNextPoint(Direction newDirection)
        {
            switch (newDirection)
            {
                case Direction.Top:
                    {
                        if (this.Direction != Direction.Bottom)
                            return new Point(this.Head.X, this.Head.Y - 1);
                        else
                            return new Point(this.Head.X, this.Head.Y + 1); //оставляем старое направление
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

        /// <summary>
        /// Поедание змейкой еды
        /// </summary>
        public void Eat()
        {
            //Добавляем последнюю удаленную точку к хвосту змейки
            this._points.Insert(0, this.lastDeletedPoint);
        }
    }
}
