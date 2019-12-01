using SnakeServer.Core.Models;

namespace SnakeServer.Core.Interfaces
{
    public interface IGameManager
    {
        /// <summary>
        /// Флаг проигрыша
        /// </summary>
        bool IsGameOver { get; }

        /// <summary>
        /// Получение точек еды на поле
        /// </summary>
        /// <returns></returns>
        Food GetFood();

        /// <summary>
        /// Получение настроек игрового поля
        /// </summary>
        /// <returns></returns>
        GameBoard GetGameBoard();

        /// <summary>
        /// Получение состояния змейки
        /// </summary>
        /// <returns></returns>
        Snake GetSnake();

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