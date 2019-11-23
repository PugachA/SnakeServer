using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnakeServer.Models;
using System;
using System.Linq;
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

        public GameBoard GameBoard { get; private set; }

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
                || (this.snake.Head.Y == this.GameBoard.GameBoardSize.Heigth - 1 && this.snakeDirection == Direction.Bottom)
                || (this.snake.Head.X == this.GameBoard.GameBoardSize.Width - 1 && this.snakeDirection == Direction.Right)
                )
            {
                logger.LogInformation("Проигрыш. Змейка врезалась в стенки");
                RestartGame();
            }

            this.snake.Move(this.snakeDirection);

            //змейка ест сама себя
            if (this.snake.Points.Where(p => p != this.snake.Head).Contains(this.snake.Head))
            {
                logger.LogInformation("Проигрыш. Змейка съела сама себя");
                RestartGame();
            }

            if (this.food.Points.First() == this.snake.Head)
            {
                // происходит поедание яблока
                this.food.GenerateFood(this.snake.Points, this.GameBoard.GameBoardSize);
            }
        }

        public void UpdateDirection(Direction newDirection)
        {
            switch (newDirection)
            {
                case Direction.Top:
                    {
                        if (this.snakeDirection != Direction.Bottom)
                            this.snakeDirection = newDirection;
                        break;
                    }
                case Direction.Bottom:
                    {
                        if (this.snakeDirection != Direction.Top)
                            this.snakeDirection = newDirection;
                        break;
                    }
                case Direction.Left:
                    {
                        if (this.snakeDirection != Direction.Right)
                            this.snakeDirection = newDirection;
                        break;
                    }

                case Direction.Right:
                    {
                        if (this.snakeDirection != Direction.Left)
                            this.snakeDirection = newDirection;
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
            this.GameBoard = new GameBoard();
            appConfiguration.Bind(this.GameBoard);

            Point middlePoint = new Point(this.GameBoard.GameBoardSize.Width / 2, this.GameBoard.GameBoardSize.Heigth / 2);
            this.snake = new Snake(middlePoint, 2);

            this.food = new Food();
            this.food.GenerateFood(this.snake.Points, this.GameBoard.GameBoardSize);

            this.snakeDirection = Direction.Top;

            this.timer = new Timer(NextTurn, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(this.GameBoard.TimeUntilNextTurnMilliseconds));
        }

        public Snake GetSnake()
        {
            return new Snake(this.snake.Points);
        }
        public Food GetFood()
        {
            return new Food(this.food.Points);
        }

    }
}
