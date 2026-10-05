using Microsoft.Extensions.Options;

using UntisSentinel.Untis;

namespace UntisSentinel;

public class Worker(ILogger<Worker> logger, MasterDataLoader masterDataLoader, IOptions<PollingOptions> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        MasterData masterData = await masterDataLoader.LoadAsync(stoppingToken);

        // periodic timer ticks at a fixed rate, so the run time doesn't add to the interval (no drift like Task.Delay has)
        using PeriodicTimer timer = new(options.Value.Interval);

        // first run right away, then every interval
        do
        {
            logger.LogInformation("Polling Timetable...");

        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
