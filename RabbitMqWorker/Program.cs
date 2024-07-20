using RabbitMqWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<RabbitMqService>();
    })
    .Build();

await host.RunAsync();