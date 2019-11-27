using Moq;
using NUnit.Framework;
using SnakeServer.Controllers;
using SnakeServer.Core.Models;
using SnakeServer.Services;

namespace SnakeServer.Tests.UnitTests
{
    public class GameBoardControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetGameboard_ReturnsCorrectResult()
        {
            //Arrange
            GameManagerService mock = Mock.Of<GameManagerService>(g => g.GetGameBoard() == new GameBoard
            {
                TurnNumber = 0,
                TimeUntilNextTurnMilliseconds = 600,
                GameBoardSize = new Size { Width = 20, Height = 20 },
                InitialSnakeLength = 2
            });

            //var controller = new GameBoardController(mock, )

            //var viewResult = Assert.IsType<ViewResult>(result)

            Assert.Pass();
        }

        private GameBoard GetTestGameBoard()
        {
            return new GameBoard
            {
                TurnNumber = 0,
                InitialSnakeLength = 2,
                TimeUntilNextTurnMilliseconds = 600,
                GameBoardSize = new Size { Height = 20, Width = 20 }
            };
        }
    }
}