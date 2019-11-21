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
        // GET: api/<controller>
        [HttpGet]
        public GameBoard Get()
        { 
            return GameBoard.Instance;
        }

        [HttpGet("snake")]
        public Snake GetSnake()
        {
            GameBoard.Instance.Snake.Add(new Point { X = 0, Y = 0 });
            return GameBoard.Instance.Snake;
        }

        [HttpGet("food")]
        public Food GetFood()
        {
            GameBoard.Instance.Food.Add(new Point { X = 4, Y = 5 });
            return GameBoard.Instance.Food;
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
