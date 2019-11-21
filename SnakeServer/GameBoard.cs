
using Newtonsoft.Json;

namespace SnakeServer
{
    public class GameBoard
    {
        [JsonProperty("turnNumber")]
        public int TurnNumber { get; set; }

        [JsonProperty("timeUntilNextTurnMilliseconds")]
        public int TimeUntilNextTurnMilliseconds { get; set; }

        [JsonProperty("gameBoardSize")]
        public Size GameBoardSize { get; set; }
    }
}
