using UntisSentinel.Untis;

namespace UntisSentinel;

public class Worker(ILogger<Worker> logger, MasterDataLoader masterDataLoader) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        MasterData masterData = await masterDataLoader.LoadAsync(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
