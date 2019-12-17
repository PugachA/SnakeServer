using SnakeServer.Core.Models;
using System.Collections.Generic;

namespace SnakeServer.DTO
{
    public class GameBoardDto
    {
        /// <summary>
        /// Номер шага змейки
        /// </summary>
        public int TurnNumber { get; set; }

        /// <summary>
        /// Период совершения шага
        /// </summary>
        public int TimeUntilNextTurnMilliseconds { get; set; }

        /// <summary>
        /// Размеры доски
        /// </summary>
        public Size GameBoardSize { get; set; }

        /// <summary>
        /// Точки из которых состоит змейка
        /// </summary>
        public IEnumerable<Point> Snake { get; set; }

        /// <summary>
        /// Точки из которых состоит еда
        /// </summary>
        public IEnumerable<Point> Food { get; set; }
    }
}
