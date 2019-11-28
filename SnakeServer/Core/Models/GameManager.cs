using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnakeServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeServer.Core.Models
{
    public class GameManager
    {
        /// <summary>
        /// Очередь направлений
        /// </summary>
        private Queue<Direction> snakeDirectionQueue;
        private Snake snake;
        private Food food;
        private GameBoard gameBoard;
        private readonly ILogger<GameManager> logger;
        public bool IsGameOver;

        public GameManager(Snake snake, Food food, GameBoard gameBoard, Direction initSnakeDirection, ILogger logger)
        {
            if (logger is null)
                throw new NullReferenceException($"Объект {nameof(logger)} не может иметь значение null и должен быть определен");

            if (snake is null)
                throw new NullReferenceException($"Объект {nameof(snake)} не может иметь значение null и должен быть определен");

            if (food is null)
                throw new NullReferenceException($"Объект {nameof(food)} не может иметь значение null и должен быть определен");

            if (gameBoard is null)
                throw new NullReferenceException($"Объект {nameof(gameBoard)} не может иметь значение null и должен быть определен");

            this.logger = logger;
            this.snake = snake;
            this.food = food;
            this.gameBoard = gameBoard;
            
            this.snakeDirectionQueue = new Queue<Direction>();
            this.snakeDirectionQueue.Enqueue(initSnakeDirection);

            //RestartGame();
        }

        /// <summary>
        /// Совершение шага в игре
        /// </summary>
        public void NextTurn()
        {
            try
            {
                if (this.IsGameOver)
                    return;

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
                    this.IsGameOver = true;
                    return;
                }

                //Змейка съела сама себя
                if (this.snake.Points.Where(p => p == this.snake.Head).Count() > 1)
                {
                    logger.LogInformation("Проигрыш. Змейка съела сама себя");
                    this.IsGameOver = true;
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
