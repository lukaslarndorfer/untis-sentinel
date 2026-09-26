using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Options;

namespace UntisSentinel.Untis;

public sealed class UntisClient
{
    private readonly HttpClient _httpClient;
    private readonly UntisOptions _options;
    private readonly string _baseUrl;
    private AuthenticateResult? _authenticationState;

    // JsonSerializerDefaults.Web for camel case
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public UntisClient(HttpClient httpClient, IOptions<UntisOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _baseUrl = $"https://{_options.Host}/WebUntis/jsonrpc.do?school={Uri.EscapeDataString(_options.School)}";
    }

    private async Task<TResult> CallAsync<TParams, TResult>(string method, TParams parameters, CancellationToken cancellationToken)
    {
        JsonRpcRequest<TParams> request = new("1", method, parameters);

        using HttpResponseMessage response =
        await _httpClient.PostAsJsonAsync(_baseUrl, request, SerializerOptions, cancellationToken);

        // throws on 4xx or 5xx
        response.EnsureSuccessStatusCode();

        JsonRpcResponse<TResult>? content = await response.Content.ReadFromJsonAsync<JsonRpcResponse<TResult>>(SerializerOptions, cancellationToken)
        ?? throw new UntisClientException("Content is null");
        if (content.Error != null)
        {
            throw new UntisClientException(content.Error.Code, content.Error.Message);
        }

        return content.Result ?? throw new UntisClientException("Response contained neither result nor error");
    }

    public async Task<AuthenticateResult> AuthenticateAsync(CancellationToken cancellationToken)
    {
        AuthenticateParams authenticateParams = new(_options.User, _options.Password, UntisOptions.ClientName);

        AuthenticateResult result = await CallAsync<AuthenticateParams, AuthenticateResult>("authenticate", authenticateParams, cancellationToken);
        _authenticationState = result;
        return result;
    }

    public async Task<List<TimetableEntry>> GetTimetableAsync(DateOnly start, DateOnly end, CancellationToken cancellationToken)
    {

        AuthenticateResult auth = _authenticationState ?? await AuthenticateAsync(cancellationToken);

        TimetableElement element = new(auth.PersonId, auth.PersonType);
        TimetableOptions options = new(element, start.ToUntisDate(), end.ToUntisDate());
        TimetableParams timetableParams = new(options);
        try
        {
            var result = await CallAsync<TimetableParams, List<TimetableEntry>>("getTimetable", timetableParams, cancellationToken);
            return result;
        }
        catch (UntisClientException ex) when (ex.Code == -8520) // not authenticated; session may have expired
        {
            await AuthenticateAsync(cancellationToken);
            var result = await CallAsync<TimetableParams, List<TimetableEntry>>("getTimetable", timetableParams, cancellationToken);
            return result;
        }

    }


}