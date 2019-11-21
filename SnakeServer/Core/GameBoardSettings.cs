using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using SnakeServer.Models;

namespace SnakeServer.Core
{
    public class GameBoardSettings
    {
        public int TurnNumber { get; set; }
        public int TimeUntilNextTurnMilliseconds { get; set; }
        public Size GameBoardSize { get; set; }
    }
}
