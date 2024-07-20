using Grpc.Core;
using GrpcService.Data;
using GrpcService.Hubs;
using Microsoft.AspNetCore.SignalR;
using Shared.DTOs;
using Shared.Protos;
using System.Text.Json;

namespace GrpcService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly GrpcServiceContext _context;
        private readonly RabbitMqService _rabbitMqService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public GreeterService(GrpcServiceContext context, RabbitMqService rabbitMqService, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _rabbitMqService = rabbitMqService;
            _hubContext = hubContext;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            // Create a DTO object
            var dto = new ExampleDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name
            };

            // Add to database
            _context.ExampleDtos.Add(dto);
            await _context.SaveChangesAsync();

            // Serialize the DTO to JSON and publish it to RabbitMQ
            var jsonMessage = JsonSerializer.Serialize(dto);
            _rabbitMqService.PublishMessage(jsonMessage);

            // For demonstration purposes, consume the message immediately
            var message = _rabbitMqService.ConsumeMessage();
            var consumedDto = JsonSerializer.Deserialize<ExampleDto>(message);

            // Notify clients through SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", consumedDto);

            // Return a HelloReply message
            return new HelloReply
            {
                Message = "Hello " + consumedDto.Name
            };
        }
    }
}