using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SnakeServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SnakeServer.Controllers
{
    [Route("api/[controller]")]
    public class GameBoardController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public GameBoard Get()
        {
            return new GameBoard
            {
                TurnNumber = 5,
                TimeUntilNextTurnMilliseconds = 600,
                GameBoardSize = new Size { Heigth = 20, Width = 20 }
            };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public Snake Get(int id)
        {
            Snake snake = new Snake();
            snake.Add(new Point { X = 0, Y = 0 });
            return snake;
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
