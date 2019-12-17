using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnakeServer.DTO;
using SnakeServer.Services;

namespace SnakeServer.Controllers
{
    [Route("api")]
    public class GameController : Controller
    {
        private readonly IGameService gameService;
        private readonly ILogger<GameController> logger;

        public GameController(IGameService gameManager, ILogger<GameController> logger)
        {
            this.gameService = gameManager;
            this.logger = logger;
        }

        // GET: api/gameboard
        [HttpGet("gameboard")]
        [ProducesResponseType(typeof(GameBoardDto), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetGameboard()
        {
            try
            {
                GameBoardDto gameBoard = new GameBoardDto
                {
                    TurnNumber = this.gameService.Game.TurnNumber,
                    TimeUntilNextTurnMilliseconds = this.gameService.Game.GameBoardSettings.TimeUntilNextTurnMilliseconds,
                    GameBoardSize = this.gameService.Game.GameBoardSettings.GameBoardSize,
                    Food = this.gameService.Game.Food,
                    Snake = this.gameService.Game.Snake
                };

                this.logger.LogInformation($"Отправляем ответ: {JsonSerializer.Serialize(gameBoard)}");
                return Ok(gameBoard);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Ошибка при обработке запроса");
                return StatusCode(500, "Ошибка при обработке запроса");
            }
        }

        // POST api/direction
        [HttpPost("direction")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult PostDirection([FromBody]DirectionDto newDirection)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                this.logger.LogInformation($"Поступил запрос {JsonSerializer.Serialize(newDirection)}");
                this.gameService.Game.UpdateDirection(newDirection.Direction);
                return Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Ошибка при обработке запроса");
                return StatusCode(500, "Ошибка при обработке запроса");
            }
        }
    }
}
