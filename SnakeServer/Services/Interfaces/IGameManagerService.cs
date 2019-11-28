using SnakeServer.Core.Models;

namespace SnakeServer.Services
{
    public interface IGameService
    {
        GameManager Game { get; }
        void Start();
        void Stop();
    }
}