using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnakeServer.Models;
using SnakeServer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SnakeServer.Controllers
{
    [Route("api")]
    public class GameBoardController : Controller
    {
        private readonly GameManagerService gameManager;
        private readonly ILogger<GameBoardController> logger;

        public GameBoardController(GameManagerService gameManager, ILogger<GameBoardController> logger)
        {
            this.gameManager = gameManager;
            this.logger = logger;
        }

        // GET: api/<controller>
        [HttpGet("gameboard")]
        public GameBoard Get()
        {
            GameBoard gameBoard = gameManager.GetGameBoard();
            this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(gameBoard)}");
            return gameBoard;
        }

        [HttpGet("snake")]
        public Snake GetSnake()
        {
            //gameManager.UpdateDirection(Direction.Bottom);
            Snake snake = gameManager.GetSnake();
            this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(snake)}");
            return snake;
        }

        [HttpGet("food")]
        public Food GetFood()
        {
            Food food = gameManager.GetFood();
            this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(food)}");
            return food;
        }

        // POST api/<controller>
        [HttpPost("direction")]
        public void Post([FromBody]DirectionObject newDirection)
        {
            gameManager.UpdateDirection(newDirection.Direction);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
