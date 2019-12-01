using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnakeServer.Core.Interfaces;
using SnakeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeServer.Services
{
    public class GameManagerService : IGameService
    {
        private readonly ILogger<GameManagerService> _logger;
        private Timer _timer;
        public IGameManager Game { get; private set; }
        public GameManagerService(ILogger<GameManagerService> logger)
        {
            _logger = logger;
        }

        public void Start()
        {
            try
            {
                _logger.LogInformation($"Запущен сервис игры {nameof(GameManagerService)}");

                //Вытягивание настроек из конфига
                IConfiguration appConfiguration = new ConfigurationBuilder().AddJsonFile("gameboardsettigs.json").Build();
                GameBoard gameBoard = new GameBoard();
                appConfiguration.Bind(gameBoard);

                //Добавление змейку в центр поля
                Point middlePoint = new Point(gameBoard.GameBoardSize.Width / 2, gameBoard.GameBoardSize.Height / 2);
                Snake snake = new Snake(middlePoint, gameBoard.InitialSnakeLength);

                //Добавление еды на поле
                Food food = new Food();
                food.GenerateFood(snake.Points, gameBoard.GameBoardSize);

                //Задание начального направления
                Queue<Direction> snakeDirectionQueue = new Queue<Direction>();
                snakeDirectionQueue.Enqueue(Direction.Top);

                this.Game = new GameManager(snake, food, gameBoard, Direction.Top, _logger);

                //запуск таймера для совершения шагов в игре
                this._timer = new Timer(
                    DoWork,
                    null,
                    TimeSpan.FromMilliseconds(gameBoard.TimeUntilNextTurnMilliseconds),
                    TimeSpan.FromMilliseconds(gameBoard.TimeUntilNextTurnMilliseconds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при старте {nameof(GameManagerService)}");
            }
        }

        public void Stop()
        {
            _logger.LogInformation($"Остановлен сервис игры {nameof(GameManagerService)}");

            _timer?.Change(Timeout.Infinite, 0);
        }

        /// <summary>
        /// Обработка шага в игры
        /// </summary>
        /// <param name="state"></param>
        private void DoWork(object state)
        {
            try
            {
                this.Game.NextTurn();

                if (this.Game.IsGameOver)
                {
                    //Перезапуск игры
                    this.Stop();
                    this.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке шага в игре");
            }
        }
    }
}
