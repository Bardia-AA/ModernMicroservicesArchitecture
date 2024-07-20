using RabbitMQ.Client;

namespace RabbitMqWorker
{
    public class RabbitMqService
    {
        public IConnection Connection { get; }
        public IModel Channel { get; }

        public RabbitMqService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();
            Channel.QueueDeclare(queue: "grpc_queue",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }
    }
}