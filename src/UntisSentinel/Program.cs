using UntisSentinel;
using UntisSentinel.Untis;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddOptions<UntisOptions>()
    .BindConfiguration(UntisOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
