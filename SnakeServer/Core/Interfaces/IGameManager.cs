using SnakeServer.Core.Models;

namespace SnakeServer.Core.Interfaces
{
    public interface IGameManager
    {
        bool IsGameOver { get; }
        Food GetFood();
        GameBoard GetGameBoard();
        Snake GetSnake();
        void NextTurn();
        void UpdateDirection(Direction newDirection);
    }
}