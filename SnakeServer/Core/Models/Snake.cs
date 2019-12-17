using SnakeServer.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeServer.Core.Models
{
    public class Snake : ISnake
    {
        /// <summary>
        /// Точки из которых состоит змейка
        /// </summary>
        private readonly List<Point> _points;

        /// <summary>
        /// Последняя удаленная точка
        /// </summary>
        private Point lastDeletedPoint;

        public Direction Direction { get; private set; }

        public IEnumerable<Point> Points => _points;

        public Point Head => _points.Last();

        public Snake()
        {
            this._points = new List<Point>();
            this.Direction = Direction.Top;
        }

        /// <summary>
        /// Создание змейки заданной длины относительно заданной точки
        /// </summary>
        /// <param name="initPoint">Начальная точка построения</param>
        /// <param name="length">Длина змейки</param>
        /// <param name="direction">Начальное направление движения змейки</param>
        public Snake(Point initPoint, int length, Direction direction = Direction.Top)
        {
            if (length < 1)
                throw new ArgumentOutOfRangeException($"Значение {nameof(length)} не может быть меньше 1");

            if (initPoint == null)
                throw new ArgumentNullException($"Значение {nameof(initPoint)} должно быть определено");

            _points = new List<Point>();

            for (int i = length - 1; i >= 0; i--)
                _points.Add(new Point(initPoint.X, initPoint.Y + i));

            this.Direction = direction;
        }

        /// <summary>
        /// Создание змйеки заданными точками
        /// </summary>
        /// <param name="points">Точки для змйеки</param>
        /// <param name="direction">Начальное направление</param>
        public Snake(IEnumerable<Point> points, Direction direction = Direction.Top)
        {
            if (points == null)
                throw new ArgumentNullException($"Значение {nameof(points)} должно быть определено");

            _points = new List<Point>(points);
            this.Direction = direction;
        }

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

        public void Eat()
        {
            if (this.lastDeletedPoint == null)
                throw new NullReferenceException("Поедание не может быть до совершения первого шага");

            //Добавляем последнюю удаленную точку к хвосту змейки
            this._points.Insert(0, this.lastDeletedPoint);
        }
    }
}
