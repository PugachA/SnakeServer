using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SnakeServer.Core;
using SnakeServer.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeServer.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(GameBoard.Instance.Settings.TimeUntilNextTurnMilliseconds));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation(
                "Timed Hosted Service is working. TurnNumber: {Count}", ++GameBoard.Instance.Settings.TurnNumber);
            GameBoard.Instance.Snake.Add(new Point { X = 1, Y = 2 });
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
