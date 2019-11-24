using System;

namespace SnakeServer.Models
{
    public class GameBoard
    {
        public GameBoard()
        {
            this.GameBoardSize = new Size();
        }

        public GameBoard(int turnNumber, int timeUntilNextTurnMilliseconds, Size gameBoardSize)
        {
            TurnNumber = turnNumber;
            TimeUntilNextTurnMilliseconds = timeUntilNextTurnMilliseconds;

            if (gameBoardSize is null)
                throw new NullReferenceException($"Значение {nameof(gameBoardSize)} должно быть определено");

            GameBoardSize = gameBoardSize;
        }

        public int TurnNumber { get; set; }
        public int TimeUntilNextTurnMilliseconds { get; set; }
        public Size GameBoardSize { get; set; }
    }
}
