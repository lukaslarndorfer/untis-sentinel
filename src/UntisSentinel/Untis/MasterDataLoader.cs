namespace UntisSentinel.Untis;

public sealed class MasterDataLoader(UntisClient client, ILogger<MasterDataLoader> logger)
{
    public async Task<MasterData> LoadAsync(CancellationToken cancellationToken)
    {
        _ = logger;
        _ = client;
        throw new NotImplementedException();
    }
    
}
