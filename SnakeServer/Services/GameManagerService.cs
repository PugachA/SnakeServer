using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnakeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeServer.Services
{
    public class TimedHostedGameService
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedGameService> _logger;
        private Timer _timer;
        private GameManager gameManager;

        public TimedHostedGameService(ILogger<TimedHostedGameService> logger)
        {
            _logger = logger;
        }

        public void Start()
        {
            _logger.LogInformation("Timed Hosted Game Service running.");

            //Вытягивание настроек из конфига
            IConfiguration appConfiguration = new ConfigurationBuilder().AddJsonFile("gameboardsettigs.json").Build();
            GameBoard gameBoard = new GameBoard();
            appConfiguration.Bind(gameBoard);

            //Добавление змейки в центр поля
            Point middlePoint = new Point(gameBoard.GameBoardSize.Width / 2, gameBoard.GameBoardSize.Height / 2);
            Snake snake = new Snake(middlePoint, gameBoard.InitialSnakeLength);

            //Добавление еды на поле
            Food food = new Food();
            food.GenerateFood(snake.Points, gameBoard.GameBoardSize);

            //Задание начального направления
            Queue<Direction> snakeDirectionQueue = new Queue<Direction>();
            snakeDirectionQueue.Enqueue(Direction.Top);

            this.gameManager = new GameManager(snake,food, gameBoard, Direction.Top, _logger); 

            //запуск таймера для совершения шагов в игре
            this._timer = new Timer(
                DoWork,
                null,
                TimeSpan.FromMilliseconds(gameBoard.TimeUntilNextTurnMilliseconds),
                TimeSpan.FromMilliseconds(gameBoard.TimeUntilNextTurnMilliseconds));
        }

        private void DoWork(object state)
        {
            this.gameManager.NextTurn();

            if (this.gameManager.IsGameOver)
            {
                this.Stop();
                this.Start();
            }

            _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", executionCount);
        }

        public void Stop()
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
