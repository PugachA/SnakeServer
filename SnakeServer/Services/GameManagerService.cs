using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnakeServer.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace SnakeServer.Services
{
    public class GameManagerService
    {
        private Direction snakeDirection;
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
            //врезание в борты
            if (
                (this.snake.Head.Y == 0 && this.snakeDirection == Direction.Top)
                || (this.snake.Head.X == 0 && this.snakeDirection == Direction.Left)
                || (this.snake.Head.Y == this.gameBoard.GameBoardSize.Heigth - 1 && this.snakeDirection == Direction.Bottom)
                || (this.snake.Head.X == this.gameBoard.GameBoardSize.Width - 1 && this.snakeDirection == Direction.Right)
                )
            {
                logger.LogInformation("Проигрыш. Змейка врезалась в стенки");
                RestartGame();
                return;
            }

            this.snake.Move(this.snakeDirection);
            this.gameBoard.TurnNumber++;

            logger.LogInformation($"Змея: {JsonSerializer.Serialize(this.snake.Points)}");

            //змейка ест сама себя
            if (this.snake.Points.Where(p => p != this.snake.Head).Contains(this.snake.Head))
            {
                logger.LogInformation("Проигрыш. Змейка съела сама себя");
                RestartGame();
                return;
            }

            if (this.food.Points.First() == this.snake.Head)
            {
                // происходит поедание яблока
                this.food.GenerateFood(this.snake.Points, this.gameBoard.GameBoardSize);
            }
        }

        public void UpdateDirection(Direction newDirection)
        {
            switch (newDirection)
            {
                case Direction.Top:
                    {
                        if (this.snake.direction != Direction.Bottom)
                        {
                            this.snakeDirection = newDirection;
                            logger.LogInformation($"Обновлено направление на {newDirection}");
                        }
                        break;
                    }
                case Direction.Bottom:
                    {
                        if (this.snake.direction != Direction.Top)
                        {
                            this.snakeDirection = newDirection;
                            logger.LogInformation($"Обновлено направление на {newDirection}");
                        }
                        break;
                    }
                case Direction.Left:
                    {
                        if (this.snake.direction != Direction.Right)
                        {
                            this.snakeDirection = newDirection;
                            logger.LogInformation($"Обновлено направление на {newDirection}");
                        }
                        break;
                    }

                case Direction.Right:
                    {
                        if (this.snake.direction != Direction.Left)
                        {
                            this.snakeDirection = newDirection;
                            logger.LogInformation($"Обновлено направление на {newDirection}");
                        }
                        break;
                    }
                default:
                    throw new NotSupportedException($"Значение '{newDirection}' не поддерживается");
            }
        }

        private void RestartGame()
        {
            if (this.timer != null)
                this.timer.Change(Timeout.Infinite, 0);

            IConfiguration appConfiguration = new ConfigurationBuilder().AddJsonFile("gameboardsettigs.json").Build();
            this.gameBoard = new GameBoard();
            appConfiguration.Bind(this.gameBoard);

            Point middlePoint = new Point(this.gameBoard.GameBoardSize.Width / 2, this.gameBoard.GameBoardSize.Heigth / 2);
            this.snake = new Snake(middlePoint, 2);

            this.food = new Food();
            this.food.GenerateFood(this.snake.Points, this.gameBoard.GameBoardSize);

            this.snakeDirection = Direction.Top;

            this.timer = new Timer(
                NextTurn,
                null,
                TimeSpan.FromMilliseconds(this.gameBoard.TimeUntilNextTurnMilliseconds),
                TimeSpan.FromMilliseconds(this.gameBoard.TimeUntilNextTurnMilliseconds));
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
