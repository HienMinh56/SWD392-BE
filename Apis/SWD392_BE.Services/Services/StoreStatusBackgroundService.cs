using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWD392_BE.Services.Interfaces;

namespace SWD392_BE.Services
{
    public class StoreStatusBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan[] _scheduledTimes;

        public StoreStatusBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _scheduledTimes = new[]
            {
                new TimeSpan(4, 1, 0),    // 4:00 AM
                new TimeSpan(8, 36, 0),   // 8:35 AM
                new TimeSpan(11, 1, 0),    // 11:00 AM
                new TimeSpan(23, 1, 0)      //test 20h
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nextRunTime = GetNextRunTime();
                var delay = nextRunTime - DateTime.Now;

                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay, stoppingToken);
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    var storeService = scope.ServiceProvider.GetRequiredService<IStoreService>();
                    await storeService.UpdateStoreStatusAsync();
                }
            }
        }

        private DateTime GetNextRunTime()
        {
            var now = DateTime.Now;
            var todayRunTimes = _scheduledTimes.Select(t => DateTime.Today.Add(t)).ToList();
            var nextRun = todayRunTimes.FirstOrDefault(t => t > now);

            if (nextRun == default)
            {
                nextRun = todayRunTimes.First().AddDays(1);
            }

            return nextRun;
        }
    }
}
