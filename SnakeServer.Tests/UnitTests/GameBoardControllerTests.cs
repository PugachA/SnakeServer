using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SnakeServer.Controllers;
using SnakeServer.Core.Interfaces;
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
        public void GetGameboard_ReturnsOkWithData()
        {
            //Arrange
            var mockGame = new Mock<IGameManager>();
            mockGame.Setup(g => g.GetGameBoard()).Returns(GetTestGameBoard());

            var mockService = new Mock<IGameService>();
            mockService.Setup(service => service.Game).Returns(mockGame.Object);

            var mockLogger = new Mock<ILogger<GameBoardController>>();

            var controller = new GameBoardController(mockService.Object, mockLogger.Object);

            //Act
            var result = controller.GetGameboard();
            
            //Assert
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.IsInstanceOf<OkObjectResult>(okObjectResult);
            Assert.IsInstanceOf<GameBoard>(okObjectResult.Value);
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