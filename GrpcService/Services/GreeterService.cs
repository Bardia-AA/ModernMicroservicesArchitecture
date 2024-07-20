using Grpc.Core;
using GrpcService.Data;
using GrpcService.Hubs;
using GrpcService.Services.Handlers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.FeatureManagement;
using Shared.Events;
using Shared.Protos;
using System.Threading.Tasks;

namespace GrpcService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly CreateExampleCommandHandler _createExampleCommandHandler;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IFeatureManager _featureManager;

        public GreeterService(CreateExampleCommandHandler createExampleCommandHandler, IHubContext<NotificationHub> hubContext, IFeatureManager featureManager)
        {
            _createExampleCommandHandler = createExampleCommandHandler;
            _hubContext = hubContext;
            _featureManager = featureManager;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            // Check if the new feature is enabled
            if (await _featureManager.IsEnabledAsync("NewFeature"))
            {
                // Handle the command
                await _createExampleCommandHandler.Handle(request.Name);

                // Notify clients through SignalR
                var exampleCreatedEvent = new ExampleCreatedEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    CreatedAt = DateTime.UtcNow
                };

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", exampleCreatedEvent);

                // Return a HelloReply message
                return new HelloReply
                {
                    Message = "Hello " + request.Name
                };
            }
            else
            {
                // Return a different message if the feature is disabled
                return new HelloReply
                {
                    Message = "Hello " + request.Name + ", but the new feature is not enabled."
                };
            }
        }
    }
}