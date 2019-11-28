using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnakeServer.Core.Models;
using SnakeServer.Services;

namespace SnakeServer.Controllers
{
    [Route("api")]
    public class GameBoardController : Controller
    {
        private readonly GameManagerService gameService;
        private readonly ILogger<GameBoardController> logger;

        public GameBoardController(GameManagerService gameManager, ILogger<GameBoardController> logger)
        {
            this.gameService = gameManager;
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
                GameBoard gameBoard = this.gameService.Game.GetGameBoard();
                this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(gameBoard)}");
                return Ok(gameBoard);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Ошибка при обработке запроса");
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
                Snake snake = this.gameService.Game.GetSnake();
                this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(snake)}");
                return Ok(snake);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Ошибка при обработке запроса");
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
                Food food = this.gameService.Game.GetFood();
                this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(food)}");
                return Ok(food);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Ошибка при обработке запроса");
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
                this.logger.LogInformation($"Поступил запрос {JsonSerializer.Serialize(newDirection)}");
                this.gameService.Game.UpdateDirection(newDirection.Direction);
                return Ok();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Ошибка при обработке запроса");
                return BadRequest("Внутрення ошибка сервера");
            }
        }
    }
}
