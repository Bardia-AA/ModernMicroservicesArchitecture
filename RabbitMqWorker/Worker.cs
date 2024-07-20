using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqWorker.Services;
using Shared.DTOs;
using System.Text;
using System.Text.Json;

namespace RabbitMqWorker
{
    public class Worker(ILogger<Worker> logger, RabbitMqService rabbitMqService) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly RabbitMqService _rabbitMqService = rabbitMqService;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var consumer = new EventingBasicConsumer(_rabbitMqService.Channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var dto = JsonSerializer.Deserialize<ExampleDto>(message);
                _logger.LogInformation("Received message: {message}", dto.Name);
            };

            _rabbitMqService.Channel.BasicConsume(queue: "grpc_queue",
                                                  autoAck: true,
                                                  consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}