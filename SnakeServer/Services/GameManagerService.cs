using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnakeServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace SnakeServer.Services
{
    public class GameManagerService
    {
        private Queue<Direction> snakeDirectionQueue;
        private Snake snake;
        private Food food;
        private Timer timer;
        private readonly ILogger<GameManagerService> logger;
        private GameBoard gameBoard;

        public GameManagerService(ILogger<GameManagerService> logger)
        {
            this.logger = logger;
            RestartGame();
        }

        public void NextTurn(object obj)
        {
            try
            {
                if (this.snakeDirectionQueue.Any())
                    this.snake.Move(this.snakeDirectionQueue.Dequeue());
                else
                    this.snake.Move(this.snake.Direction);

                this.gameBoard.TurnNumber++;

                //врезание в борты
                if (
                    (this.snake.Head.Y == -1)
                    || (this.snake.Head.X == -1)
                    || (this.snake.Head.Y == this.gameBoard.GameBoardSize.Heigth)
                    || (this.snake.Head.X == this.gameBoard.GameBoardSize.Width)
                    )
                {
                    logger.LogInformation("Проигрыш. Змейка врезалась в стенки");
                    RestartGame();
                    return;
                }

                //змейка ест сама себя
                if (this.snake.Points.Where(p => p == this.snake.Head).Count() > 1)
                {
                    logger.LogInformation("Проигрыш. Змейка съела сама себя");
                    RestartGame();
                    return;
                }

                if (this.food.Points.Contains(this.snake.Head))
                {
                    // происходит поедание яблока
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

        public void UpdateDirection(Direction newDirection)
        {
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

        private void RestartGame()
        {
            try
            {
                if (this.timer != null)
                    this.timer.Change(Timeout.Infinite, 0);

                IConfiguration appConfiguration = new ConfigurationBuilder().AddJsonFile("gameboardsettigs.json").Build();
                this.gameBoard = new GameBoard();
                appConfiguration.Bind(this.gameBoard);

                Point middlePoint = new Point(this.gameBoard.GameBoardSize.Width / 2, this.gameBoard.GameBoardSize.Heigth / 2);
                this.snake = new Snake(middlePoint, this.gameBoard.InitialSnakeLength);

                this.food = new Food();
                this.food.GenerateFood(this.snake.Points, this.gameBoard.GameBoardSize);

                this.snakeDirectionQueue = new Queue<Direction>();
                this.snakeDirectionQueue.Enqueue(Direction.Top);

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

        public Snake GetSnake()
        {
            return new Snake(this.snake.Points);
        }
        public Food GetFood()
        {
            return new Food(this.food.Points);
        }

        public GameBoard GetGameBoard()
        {
            return new GameBoard(this.gameBoard.TurnNumber, this.gameBoard.TimeUntilNextTurnMilliseconds, this.gameBoard.GameBoardSize);
        }
    }
}
