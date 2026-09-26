using System.Net;

using Microsoft.Extensions.Options;

using UntisSentinel;
using UntisSentinel.Untis;

var builder = Host.CreateApplicationBuilder(args);
var cookieContainer = new CookieContainer();

builder.Services
    .AddOptions<UntisOptions>()
    .BindConfiguration(UntisOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<UntisClient>(http =>
{
    http.Timeout = TimeSpan.FromSeconds(30);
}).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    UseCookies = true,
    CookieContainer = cookieContainer
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
