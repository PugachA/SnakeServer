using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnakeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace SnakeServer.Services
{
    public class GameManagerService
    {
        /// <summary>
        /// Очередь направлений
        /// </summary>
        private Queue<Direction> snakeDirectionQueue;
        private Snake snake;
        private Food food;
        private Timer timer;
        private GameBoard gameBoard;
        private readonly ILogger<GameManagerService> logger;

        public GameManagerService(ILogger<GameManagerService> logger)
        {
            this.logger = logger;
            RestartGame();
        }

        /// <summary>
        /// Совершение шага в игре
        /// </summary>
        /// <param name="obj"></param>
        private void NextTurn(object obj)
        {
            try
            {
                //Если очередь пустая, двигайся в старом направлении
                if (this.snakeDirectionQueue.Any())
                    this.snake.Move(this.snakeDirectionQueue.Dequeue());
                else
                    this.snake.Move(this.snake.Direction);

                this.gameBoard.TurnNumber++;

                //Попадание в стенки
                if (
                    (this.snake.Head.Y == -1)
                    || (this.snake.Head.X == -1)
                    || (this.snake.Head.Y == this.gameBoard.GameBoardSize.Height)
                    || (this.snake.Head.X == this.gameBoard.GameBoardSize.Width)
                    )
                {
                    logger.LogInformation("Проигрыш. Змейка врезалась в стенки");
                    RestartGame();
                    return;
                }

                //Змейка съела сама себя
                if (this.snake.Points.Where(p => p == this.snake.Head).Count() > 1)
                {
                    logger.LogInformation("Проигрыш. Змейка съела сама себя");
                    RestartGame();
                    return;
                }

                //Змейка ест вкусняшку
                if (this.food.Points.Contains(this.snake.Head))
                {
                    logger.LogInformation($"Змейка сьела {JsonSerializer.Serialize(this.snake.Head)}");
                    this.snake.Eat();
                    this.food.DeleteFood(this.snake.Head);
                    this.food.GenerateFood(this.snake.Points, this.gameBoard.GameBoardSize);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при выполнении шага в игре");
            }
        }

        /// <summary>
        /// Добавление в очередь нового направления
        /// </summary>
        /// <param name="newDirection"></param>
        public void UpdateDirection(Direction newDirection)
        {
            //Последнее направление
            Direction lastDirection = this.snakeDirectionQueue.Any() ? this.snakeDirectionQueue.Last() : this.snake.Direction;

            switch (newDirection)
            {
                case Direction.Top:
                case Direction.Bottom:
                    {
                            if (lastDirection == Direction.Right || lastDirection == Direction.Left)
                            {
                                this.snakeDirectionQueue.Enqueue(newDirection);
                                logger.LogInformation($"Обновлено направление на {newDirection}");
                            }
                            break;
                    }
                case Direction.Left:
                case Direction.Right:
                    {
                        if (lastDirection == Direction.Top || lastDirection == Direction.Bottom)
                        {
                            this.snakeDirectionQueue.Enqueue(newDirection);
                            logger.LogInformation($"Обновлено направление на {newDirection}");
                        }
                        break;
                    }
                default:
                    {
                        logger.LogError($"Значение '{newDirection}' не поддерживается");
                        break;
                    }
            }
        }
        
        /// <summary>
        /// Перезагрузка игры
        /// </summary>
        private void RestartGame()
        {
            try
            {
                //Остановка таймера
                if (this.timer != null)
                    this.timer.Change(Timeout.Infinite, 0);
                
                //Вытягивание настроек из конфига
                IConfiguration appConfiguration = new ConfigurationBuilder().AddJsonFile("gameboardsettigs.json").Build();
                this.gameBoard = new GameBoard();
                appConfiguration.Bind(this.gameBoard);

                //Добавление змейки в центр поля
                Point middlePoint = new Point(this.gameBoard.GameBoardSize.Width / 2, this.gameBoard.GameBoardSize.Height / 2);
                this.snake = new Snake(middlePoint, this.gameBoard.InitialSnakeLength);

                //Добавление еды на поле
                this.food = new Food();
                this.food.GenerateFood(this.snake.Points, this.gameBoard.GameBoardSize);

                //Задание начального направления
                this.snakeDirectionQueue = new Queue<Direction>();
                this.snakeDirectionQueue.Enqueue(Direction.Top);

                //запуск таймера для совершения шагов в игре
                this.timer = new Timer(
                    NextTurn,
                    null,
                    TimeSpan.FromMilliseconds(this.gameBoard.TimeUntilNextTurnMilliseconds),
                    TimeSpan.FromMilliseconds(this.gameBoard.TimeUntilNextTurnMilliseconds));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при перезагрузке игры");
            }
        }

        /// <summary>
        /// Получение состояния змейки
        /// </summary>
        /// <returns></returns>
        public Snake GetSnake()
        {
            return new Snake(this.snake.Points);
        }

        /// <summary>
        /// Получение точек еды на поле
        /// </summary>
        /// <returns></returns>
        public Food GetFood()
        {
            return new Food(this.food.Points);
        }

        /// <summary>
        /// Получение настроек игрового поля
        /// </summary>
        /// <returns></returns>
        public GameBoard GetGameBoard()
        {
            return new GameBoard(this.gameBoard.TurnNumber, this.gameBoard.TimeUntilNextTurnMilliseconds, this.gameBoard.GameBoardSize);
        }
    }
}
