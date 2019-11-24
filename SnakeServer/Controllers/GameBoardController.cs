using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnakeServer.Models;
using SnakeServer.Services;

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

        // GET: api/gameboard
        [HttpGet("gameboard")]
        [ProducesResponseType(typeof(GameBoard), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetGameboard()
        {
            try
            {
                GameBoard gameBoard = gameManager.GetGameBoard();
                this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(gameBoard)}");
                return Ok(gameBoard);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Ошибка при обработке запроса");
                return BadRequest("Внутрення ошибка сервера");
            }
        }

        // GET: api/snake
        [HttpGet("snake")]
        [ProducesResponseType(typeof(Snake), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetSnake()
        {
            try
            {
                Snake snake = gameManager.GetSnake();
                this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(snake)}");
                return Ok(snake);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обработке запроса");
                return BadRequest("Внутрення ошибка сервера");
            }
        }

        // GET: api/food
        [HttpGet("food")]
        [ProducesResponseType(typeof(Food), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetFood()
        {
            try
            {
                Food food = gameManager.GetFood();
                this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(food)}");
                return Ok(food);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обработке запроса");
                return BadRequest("Внутрення ошибка сервера");
            }
        }

        // POST api/direction
        [HttpPost("direction")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PostDirection([FromBody]DirectionObject newDirection)
        {
            try
            {
                logger.LogInformation($"Поступил запрос {JsonSerializer.Serialize(newDirection)}");
                gameManager.UpdateDirection(newDirection.Direction);
                return Ok();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Ошибка при обработке запроса");
                return BadRequest("Внутрення ошибка сервера");
            }
        }
    }
}
