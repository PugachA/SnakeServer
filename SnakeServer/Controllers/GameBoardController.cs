using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        public string Get(int id)
        {
            return "value";
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
