# ModernMicroservicesArchitecture

## Overview

This project demonstrates a comprehensive microservices architecture using various modern technologies. It includes gRPC for inter-service communication, RabbitMQ for messaging, SignalR for real-time notifications, GraphQL for querying, and an API Gateway using Ocelot. The solution is structured into multiple projects, each serving a distinct purpose within the architecture.

## Solution Structure

### Projects

1. **ApiGateway**
   - **Purpose**: Acts as the API Gateway using Ocelot to route requests to appropriate services.
   - **Key Files**:
     - `Program.cs`: Configures Ocelot middleware.
     - `ocelot.json`: Defines routing rules for Ocelot.

2. **AuthService**
   - **Purpose**: Handles user authentication using JWT and exposes a GraphQL endpoint.
   - **Key Files**:
     - `Controllers/AuthController.cs`: Manages authentication endpoints.
     - `GraphQL/Query.cs`: Defines GraphQL queries.
     - `GraphQL/UserType.cs`: Represents user data for GraphQL.
     - `Models/LoginRequest.cs`: Represents the login request model.
     - `Program.cs`: Configures authentication, GraphQL, and Swagger.

3. **GrpcService**
   - **Purpose**: Provides gRPC services and integrates RabbitMQ for messaging and SignalR for real-time notifications.
   - **Key Files**:
     - `Hubs/NotificationHub.cs`: Manages SignalR connections and methods.
     - `Services/GreeterService.cs`: Implements gRPC service logic.
     - `Services/RabbitMqService.cs`: Manages RabbitMQ connections and messaging.
     - `Protos/greet.proto`: Defines the gRPC service and messages.
     - `Data/GrpcServiceContext.cs`: Entity Framework DbContext for database access.
     - `Program.cs`: Configures gRPC, SignalR, RabbitMQ, and EF Core.

4. **RabbitMqWorker**
   - **Purpose**: Background service to process messages from RabbitMQ.
   - **Key Files**:
     - `Worker.cs`: Implements the background worker logic.
     - `Services/RabbitMqService.cs`: Manages RabbitMQ connections and messaging.
     - `Program.cs`: Configures the worker service.

5. **Shared**
   - **Purpose**: Contains shared DTOs and common code used across services.
   - **Key Files**:
     - `DTOs/ExampleDto.cs`: Defines a shared data transfer object.

## Technologies Used

- **gRPC**: For high-performance, language-agnostic RPC communication.
- **RabbitMQ**: For asynchronous messaging between services.
- **SignalR**: For real-time notifications.
- **GraphQL**: For flexible and efficient data querying.
- **Ocelot**: For API Gateway functionality.
- **Entity Framework Core**: For database access.
- **JWT Authentication**: For secure user authentication.
- **Swagger**: For API documentation.

## Setup and Usage

### Prerequisites

- .NET 8 SDK
- RabbitMQ Server
- SQL Server or another supported database

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Bardia-AA/ModernMicroservicesArchitecture.git
   cd ModernMicroservicesArchitecture
   ```

2. **Start RabbitMQ Server**:
   Ensure RabbitMQ is installed and running. Refer to [RabbitMQ Installation Guide](https://www.rabbitmq.com/download.html) for setup instructions.

3. **Configure Connection Strings**:
   Update the connection string in `GrpcService/appsettings.json` to point to your database.
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=GrpcServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

4. **Build the solution**:
   Open the solution in Visual Studio and build it to restore all dependencies.

### Running the Services

1. **ApiGateway**:
   - Set `ApiGateway` as the startup project and run it.
   - This will start the API Gateway using Ocelot.

2. **AuthService**:
   - Set `AuthService` as the startup project and run it.
   - Access Swagger UI at `https://localhost:5002/swagger` to explore authentication endpoints.

3. **GrpcService**:
   - Set `GrpcService` as the startup project and run it.
   - The gRPC service will be available at `https://localhost:5001`.

4. **RabbitMqWorker**:
   - Set `RabbitMqWorker` as the startup project and run it.
   - This will start the background worker to process RabbitMQ messages.

### How It Works

- **ApiGateway**: Routes requests to the appropriate services based on `ocelot.json` configuration.
- **AuthService**:
  - Provides JWT authentication endpoints (`/Auth/token`).
  - Exposes a GraphQL endpoint (`/graphql`) to query user data.
- **GrpcService**:
  - Implements a gRPC service (`GreeterService`) with methods defined in `greet.proto`.
  - Publishes and consumes messages from RabbitMQ.
  - Sends real-time notifications to connected clients via SignalR.
- **RabbitMqWorker**: Listens to RabbitMQ messages and processes them, demonstrating background task processing.

### Example Requests

1. **Authenticate and Get JWT Token**:
   - Send a POST request to `https://localhost:5002/Auth/token` with the following JSON body:
     ```json
     {
       "Username": "test",
       "Password": "password"
     }
     ```

2. **Call gRPC Service**:
   - Use a gRPC client tool like [grpcurl](https://github.com/fullstorydev/grpcurl) to call the `SayHello` method on the `GrpcService`.

3. **Subscribe to SignalR Notifications**:
   - Connect to `https://localhost:5001/hubs/notification` using a SignalR client to receive real-time messages.

4. **Query Data via GraphQL**:
   - Send a POST request to `https://localhost:5002/graphql` with the following GraphQL query:
     ```graphql
     {
       user {
         username
       }
     }
     ```

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.

## License

This project is licensed under the MIT License.
