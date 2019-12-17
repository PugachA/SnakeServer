using SnakeServer.Core.Models;
using System.Collections.Generic;

namespace SnakeServer.Core.Interfaces
{
    public interface IGameManager
    {
        /// <summary>
        /// Флаг проигрыша
        /// </summary>
        bool IsGameOver { get; }
        
        /// <summary>
        /// Номер шага в игре 
        /// </summary>
        int TurnNumber { get; }

        /// <summary>
        /// Получение точек еды на поле
        /// </summary>
        IEnumerable<Point> Food { get; }

        /// <summary>
        /// Получение настроек игрового поля
        /// </summary>
        GameBoardSettings GameBoardSettings { get; }

        /// <summary>
        /// Получение точек змейки
        /// </summary>
        IEnumerable<Point> Snake { get; }

        /// <summary>
        /// Совершение шага в игре
        /// </summary>
        void NextTurn();

        /// <summary>
        /// Добавление в очередь нового направления
        /// </summary>
        /// <param name="newDirection"></param>
        void UpdateDirection(Direction newDirection);
    }
}