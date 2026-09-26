using System.Text.Json;

using Microsoft.Extensions.Options;

namespace UntisSentinel.Untis;

public sealed class UntisClient
{
    private HttpClient _httpClient;
    private IOptions<UntisOptions> _options;

    public UntisClient(HttpClient httpClient, IOptions<UntisOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    // JsonSerializerDefaults.Web for camel case
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
}