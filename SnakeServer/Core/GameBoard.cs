using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SnakeServer.Models;
using System.Text.Json.Serialization;

namespace SnakeServer.Core
{
    public class GameBoard
    {
        private static volatile GameBoard _instance;
        private static readonly object InstanceLock = new object();

        public GameBoardSettings Settings { get; private set; }

        [JsonIgnore]
        public Snake Snake { get; }

        [JsonIgnore]
        public Food Food { get; }

        private GameBoard()
        {
            IConfiguration appConfiguration = new ConfigurationBuilder().AddJsonFile("gameboardsettigs.json").Build();
            this.Settings = new GameBoardSettings();
            appConfiguration.Bind(this.Settings);

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
