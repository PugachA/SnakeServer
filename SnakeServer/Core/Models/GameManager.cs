using Microsoft.Extensions.Logging;
using SnakeServer.Core.Interfaces;
using SnakeServer.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SnakeServer.Core.Models
{
    public class GameManager : IGameManager
    {
        /// <summary>
        /// Очередь направлений
        /// </summary>
        private readonly Queue<Direction> _snakeDirectionQueue;
        private readonly ISnake _snake;
        private readonly Food _food;
        private readonly GameBoardSettings _gameBoardSettings;
        private readonly ILogger _logger;

        public bool IsGameOver { get; private set; }
        public int TurnNumber { get; private set; }
        public IEnumerable<Point> Snake { get; private set; }
        public IEnumerable<Point> Food => _food.Points;
        public GameBoardSettings GameBoardSettings => new GameBoardSettings(_gameBoardSettings);

        public GameManager(ISnake snake, Food food, GameBoardSettings gameBoardSettings, Direction initSnakeDirection, ILogger logger)
        {
            if (logger is null)
                throw new ArgumentNullException($"Объект {nameof(logger)} не может иметь значение null и должен быть определен");

            if (snake is null)
                throw new ArgumentNullException($"Объект {nameof(snake)} не может иметь значение null и должен быть определен");

            if (food is null)
                throw new ArgumentNullException($"Объект {nameof(food)} не может иметь значение null и должен быть определен");

            if (gameBoardSettings is null)
                throw new ArgumentNullException($"Объект {nameof(gameBoardSettings)} не может иметь значение null и должен быть определен");

            this._snake = snake;
            this.Snake = snake.Points;
            
            this._food = food;
            this._gameBoardSettings = gameBoardSettings;

            this._snakeDirectionQueue = new Queue<Direction>();
            this._snakeDirectionQueue.Enqueue(initSnakeDirection);
            this._logger = logger;
        }

        public void NextTurn()
        {
            try
            {
                if (this.IsGameOver)
                    return;

                //Если очередь пустая, двигайся в старом направлении 
                if (this._snakeDirectionQueue.Any())
                    this._snake.Move(this._snakeDirectionQueue.Dequeue());
                else
                    this._snake.Move(this._snake.Direction);

                this.TurnNumber++;

                //Попадание в стенки
                if (
                    (this._snake.Head.Y == -1)
                    || (this._snake.Head.X == -1)
                    || (this._snake.Head.Y == this._gameBoardSettings.GameBoardSize.Height)
                    || (this._snake.Head.X == this._gameBoardSettings.GameBoardSize.Width)
                    )
                {
                    _logger.LogInformation("Проигрыш. Змейка врезалась в стенки");
                    this.IsGameOver = true;
                }

                //Змейка съела сама себя
                if (this._snake.Points.Where(p => p == this._snake.Head).Count() > 1)
                {
                    _logger.LogInformation("Проигрыш. Змейка съела сама себя");
                    this.IsGameOver = true;
                }

                //Змейка ест вкусняшку
                if (this._food.Points.Contains(this._snake.Head))
                {
                    _logger.LogInformation($"Змейка сьела {JsonSerializer.Serialize(this._snake.Head)}");
                    this._snake.Eat();
                    this._food.DeleteFood(this._snake.Head);
                    this._food.GenerateFood(this._snake.Points, this._gameBoardSettings.GameBoardSize);
                }

                if (!IsGameOver)
                    this.Snake = new List<Point>(this._snake.Points);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении шага в игре");
            }
        }

        public void UpdateDirection(Direction newDirection)
        {
            //Последнее направление
            Direction lastDirection = this._snakeDirectionQueue.Any() ? this._snakeDirectionQueue.Last() : this._snake.Direction;

            switch (newDirection)
            {
                case Direction.Top:
                case Direction.Bottom:
                    {
                        if (lastDirection == Direction.Right || lastDirection == Direction.Left)
                        {
                            this._snakeDirectionQueue.Enqueue(newDirection);
                            _logger.LogInformation($"Обновлено направление на {newDirection}");
                        }
                        break;
                    }
                case Direction.Left:
                case Direction.Right:
                    {
                        if (lastDirection == Direction.Top || lastDirection == Direction.Bottom)
                        {
                            this._snakeDirectionQueue.Enqueue(newDirection);
                            _logger.LogInformation($"Обновлено направление на {newDirection}");
                        }
                        break;
                    }
                default:
                    {
                        _logger.LogError($"Значение '{newDirection}' не поддерживается");
                        break;
                    }
            }
        }
    }
}
