using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeServer.Models
{
    public class DirectionObject
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction Direction { get; set; }
    }
}
