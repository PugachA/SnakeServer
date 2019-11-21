using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeServer
{
    public class Food
    {
        [JsonPropertyName("food")]
        public List<Point> Points { get; set; }

        public void AddFood(Point point)
        {

        }

        public bool TryDelete(Point point)
        {
            return true;
        }
        
    }
}
