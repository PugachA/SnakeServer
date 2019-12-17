using System.Collections.Generic;

namespace SnakeServer.Core.Models.Interfaces
{
    public interface ISnake
    {
        /// <summary>
        /// Текущее направление змейки
        /// </summary>
        Direction Direction { get; }

        /// <summary>
        /// Точка соответствующая голове змейки
        /// </summary>
        Point Head { get; }

        /// <summary>
        /// Свойство для инкапсуляции точек змейки
        /// </summary>
        IEnumerable<Point> Points { get; }

        /// <summary>
        /// Поедание змейкой еды
        /// </summary>
        void Eat();

        /// <summary>
        /// Совершение шага змейкой
        /// </summary>
        /// <param name="newDirection">Новое направление движения</param>
        void Move(Direction newDirection);
    }
}