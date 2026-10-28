using EaaaTrackApi.Context;
using MigrationWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<SubscriptionContext>("subscriptiondb");
var host = builder.Build();
host.Run();