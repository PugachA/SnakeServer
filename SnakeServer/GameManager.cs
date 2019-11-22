using Microsoft.Extensions.Configuration;
using SnakeServer.Core;
using SnakeServer.Models;
using System;
using System.Linq;
using System.Threading;

namespace SnakeServer
{
    public class GameManager
    {
        public Direction SnakeDirection { get; private set; }
        public GameBoard GameBoard { get; private set; }
        private Snake snake;
        private Food food;

        private Timer timer;

        public GameManager()
        {
            RestartGame();
        }

        public void NextTurn(object obj)
        {
            this.snake.Move(this.SnakeDirection);

            if (
                this.snake.Points.Last().X == this.GameBoard.GameBoardSize.Width
                || this.snake.Points.Last().Y == this.GameBoard.GameBoardSize.Heigth
                || this.snake.Points.Last().Y == -1
                || this.snake.Points.Last().X == -1
                || this.food.Points.Contains(this.snake.Points.Last())
                )
                RestartGame();

            if (this.food.Points.Contains(this.snake.Points.Last()))
            {
                // происходит поедание яблока
                this.food.GenerateFood(this.snake.Points, this.GameBoard.GameBoardSize);
            }
        }

        public void UpdateDirection(Direction newDirection)
        {
            this.SnakeDirection = newDirection;
        }

        private void RestartGame()
        {
            if(this.timer != null)
                this.timer.Change(Timeout.Infinite, 0);

            IConfiguration appConfiguration = new ConfigurationBuilder().AddJsonFile("gameboardsettigs.json").Build();
            this.GameBoard = new GameBoard();
            appConfiguration.Bind(this.GameBoard);

            Point middlePoint = new Point(this.GameBoard.GameBoardSize.Width / 2, this.GameBoard.GameBoardSize.Heigth / 2);
            this.snake = new Snake(middlePoint, 2);

            this.food = new Food();
            this.food.GenerateFood(this.snake.Points, this.GameBoard.GameBoardSize);

            this.SnakeDirection = Direction.Top;

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
