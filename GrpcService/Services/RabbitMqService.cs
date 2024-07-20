using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GrpcService.Services
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "grpc_queue",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                routingKey: "grpc_queue",
                                basicProperties: null,
                                body: body);
        }

        public string ConsumeMessage()
        {
            var consumer = new EventingBasicConsumer(_channel);
            var message = "";
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
            };
            _channel.BasicConsume(queue: "grpc_queue",
                                autoAck: true,
                                consumer: consumer);
            return message;
        }
    }
}