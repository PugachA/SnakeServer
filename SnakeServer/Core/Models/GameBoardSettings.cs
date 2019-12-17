using System;

namespace SnakeServer.Core.Models
{
    public class GameBoardSettings
    {
        /// <summary>
        /// Период совершения шага
        /// </summary>
        public int TimeUntilNextTurnMilliseconds { get; set; }

        /// <summary>
        /// Начальная длина змейки
        /// </summary>
        public int InitialSnakeLength { get; set; }

        /// <summary>
        /// Размеры доски
        /// </summary>
        public Size GameBoardSize { get; set; }

        public GameBoardSettings()
        {
            this.GameBoardSize = new Size();
        }

        public GameBoardSettings(int timeUntilNextTurnMilliseconds, Size gameBoardSize, int initialSnakeLength = 2)
        {
            this.TimeUntilNextTurnMilliseconds = timeUntilNextTurnMilliseconds;
            this.InitialSnakeLength = initialSnakeLength;

            if (gameBoardSize is null)
                throw new ArgumentNullException($"Значение '{nameof(gameBoardSize)}' должно быть определено");

            this.GameBoardSize = gameBoardSize;
        }

        public GameBoardSettings(GameBoardSettings gameBoardSettings)
        {
            if (gameBoardSettings is null)
                throw new ArgumentNullException($"Значение '{nameof(gameBoardSettings)}' должно быть определено");

            if (gameBoardSettings.GameBoardSize is null)
                throw new ArgumentNullException($"Значение '{nameof(gameBoardSettings.GameBoardSize)}' должно быть определено");

            this.TimeUntilNextTurnMilliseconds = gameBoardSettings.TimeUntilNextTurnMilliseconds;
            this.InitialSnakeLength = gameBoardSettings.InitialSnakeLength;
            this.GameBoardSize = gameBoardSettings.GameBoardSize;
        }
    }
}
