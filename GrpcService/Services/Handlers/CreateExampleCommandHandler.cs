using GrpcService.Data;
using Shared.DTOs;
using Shared.Events;
using System.Text.Json;

namespace GrpcService.Services.Handlers
{
    public class CreateExampleCommandHandler(GrpcServiceContext sqlContext, MongoDbContext mongoContext, RabbitMqService rabbitMqService)
    {
        private readonly GrpcServiceContext _sqlContext = sqlContext;
        private readonly MongoDbContext _mongoContext = mongoContext;
        private readonly RabbitMqService _rabbitMqService = rabbitMqService;

        public async Task Handle(string name)
        {
            // Create a DTO object
            var dto = new ExampleDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = name
            };

            // Add to SQL Server
            _sqlContext.ExampleDtos.Add(dto);
            await _sqlContext.SaveChangesAsync();

            // Add to MongoDB
            await _mongoContext.ExampleDtos.InsertOneAsync(dto);

            // Create an event
            var exampleCreatedEvent = new ExampleCreatedEvent
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow
            };

            // Serialize the event to JSON and publish it to RabbitMQ
            var jsonMessage = JsonSerializer.Serialize(exampleCreatedEvent);
            _rabbitMqService.PublishMessage(jsonMessage);
        }
    }
}