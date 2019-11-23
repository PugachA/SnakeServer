namespace SnakeServer.Models
{
    public class GameBoard
    {
        public int TurnNumber { get; set; }
        public int TimeUntilNextTurnMilliseconds { get; set; }
        public Size GameBoardSize { get; set; }
    }
}
