using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnakeServer.Models;
using SnakeServer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SnakeServer.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet]
        public GameBoard Get()
        {
            this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(gameManager.GameBoard)}");
            return gameManager.GameBoard;
        }

        [HttpGet("snake")]
        public Snake GetSnake()
        {
            //gameManager.NextTurn();
            this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(gameManager.GetSnake())}");
            return gameManager.GetSnake();
        }

        [HttpGet("food")]
        public Food GetFood()
        {
            this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(gameManager.GetFood())}");
            return gameManager.GetFood();
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
