using SnakeServer.Core.Interfaces;
using SnakeServer.Core.Models;

namespace SnakeServer.Services
{
    public interface IGameService
    {
        IGameManager Game { get; }
        void Start();
        void Stop();
    }
}