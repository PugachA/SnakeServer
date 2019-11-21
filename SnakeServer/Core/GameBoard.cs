using SnakeServer.Models;
using System.Text.Json.Serialization;

namespace SnakeServer.Core
{
    public class GameBoard
    {
        private static volatile GameBoard _instance;
        private static readonly object InstanceLock = new object();

        public int TurnNumber { get; set; }
        public int TimeUntilNextTurnMilliseconds { get; set; }
        public Size GameBoardSize { get; set; }
        [JsonIgnore]
        public Snake Snake { get; }
        [JsonIgnore]
        public Food Food { get; }

        private GameBoard() 
        {
            this.TurnNumber = 0;
            this.TimeUntilNextTurnMilliseconds = 600;
            this.GameBoardSize = new Size { Heigth = 20, Width = 20 };
            this.Snake = new Snake();
            this.Food = new Food();
        }

        public static GameBoard Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GameBoard();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
