using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.DTOs;
using System.Text;
using System.Text.Json;

namespace RabbitMqWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqService _rabbitMqService;

        public Worker(ILogger<Worker> logger, RabbitMqService rabbitMqService)
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;
        }

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