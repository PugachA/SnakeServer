using System;
using System.Text.Json.Serialization;

namespace SnakeServer.Models
{
    public class GameBoard
    {
        public GameBoard()
        {
            this.GameBoardSize = new Size();
            this.InitialSnakeLength = 2;
        }

        public GameBoard(int turnNumber, int timeUntilNextTurnMilliseconds, Size gameBoardSize, int initialSnakeLength = 2)
        {
            this.TurnNumber = turnNumber;
            this.TimeUntilNextTurnMilliseconds = timeUntilNextTurnMilliseconds;
            this.InitialSnakeLength = initialSnakeLength;

            if (gameBoardSize is null)
                throw new NullReferenceException($"Значение '{nameof(gameBoardSize)}' должно быть определено");

            this.GameBoardSize = gameBoardSize;
        }

        public int TurnNumber { get; set; }
        public int TimeUntilNextTurnMilliseconds { get; set; }
        [JsonIgnore]
        public int InitialSnakeLength { get; set; }
        public Size GameBoardSize { get; set; }
    }
}
