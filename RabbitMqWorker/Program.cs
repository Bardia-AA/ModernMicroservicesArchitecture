using RabbitMqWorker;
using RabbitMqWorker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<RabbitMqService>();
    })
    .Build();

await host.RunAsync();