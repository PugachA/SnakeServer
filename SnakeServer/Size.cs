using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeServer
{
    public class Size
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("heigth")]
        public int Heigth { get; set; }
    }
}
