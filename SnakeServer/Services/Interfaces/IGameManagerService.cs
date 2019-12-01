using SnakeServer.Core.Interfaces;
using SnakeServer.Core.Models;

namespace SnakeServer.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Игра для запуска
        /// </summary>
        IGameManager Game { get; }

        /// <summary>
        /// Инициализация и запуск игры по таймеру
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка сервиса игры
        /// </summary>
        void Stop();
    }
}