using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SnakeServer.Core;
using SnakeServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SnakeServer.Controllers
{
    [Route("api/[controller]")]
    public class GameBoardController : Controller
    {
        private readonly GameManager gameManager;

        public GameBoardController(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        // GET: api/<controller>
        [HttpGet]
        public GameBoard Get()
        {
            return gameManager.GameBoard;
        }

        [HttpGet("snake")]
        public Snake GetSnake()
        {
            //gameManager.NextTurn();
            return gameManager.GetSnake();
        }

        [HttpGet("food")]
        public Food GetFood()
        {
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
