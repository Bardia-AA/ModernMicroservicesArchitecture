# ModernMicroservicesArchitecture

## Overview

This project demonstrates a comprehensive microservices architecture using various modern technologies. It includes gRPC for inter-service communication, RabbitMQ for messaging, SignalR for real-time notifications, GraphQL for querying, API Gateway using Ocelot, IdentityServer4 for authentication, and feature management using Microsoft.FeatureManagement. The solution is structured into multiple projects, each serving a distinct purpose within the architecture.

## Solution Structure

```plaintext
ModernMicroservicesArchitecture
├── ApiGateway
│   ├── Program.cs
│   └── ocelot.json
├── AuthService
│   ├── Controllers
│   │   └── AuthController.cs
│   ├── GraphQL
│   │   ├── Query.cs
│   │   └── UserType.cs
│   ├── Models
│   │   └── LoginRequest.cs
│   └── Program.cs
├── GrpcService
│   ├── Controllers
│   │   └── ExampleController.cs
│   ├── Hubs
│   │   └── NotificationHub.cs
│   ├── Services
│   │   ├── GreeterService.cs
│   │   ├── Handlers
│   │   │   └── CreateExampleCommandHandler.cs
│   │   └── RabbitMqService.cs
│   ├── Protos
│   │   └── greet.proto
│   ├── Data
│   │   ├── GrpcServiceContext.cs
│   │   └── MongoDbContext.cs
│   └── Program.cs
├── RabbitMqWorker
│   ├── Worker.cs
│   ├── Services
│   │   └── RabbitMqService.cs
│   └── Program.cs
├── Shared
│   ├── DTOs
│   │   └── ExampleDto.cs
│   └── Events
│       └── ExampleCreatedEvent.cs
└── IdentityServer
    ├── Config.cs
    └── Program.cs
```

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
   - **Purpose**: Provides gRPC services and integrates RabbitMQ for messaging, SignalR for real-time notifications, and supports MongoDB and SQL Server for database operations.
   - **Key Files**:
     - `Controllers/ExampleController.cs`: API versioned controller to demonstrate versioning.
     - `Hubs/NotificationHub.cs`: Manages SignalR connections and methods.
     - `Services/GreeterService.cs`: Implements gRPC service logic.
     - `Handlers/CreateExampleCommandHandler.cs`: Handles creation commands for event sourcing.
     - `Services/RabbitMqService.cs`: Manages RabbitMQ connections and messaging.
     - `Protos/greet.proto`: Defines the gRPC service and messages.
     - `Data/GrpcServiceContext.cs`: Entity Framework DbContext for SQL Server access.
     - `Data/MongoDbContext.cs`: MongoDB context for database access.
     - `Program.cs`: Configures gRPC, SignalR, RabbitMQ, EF Core, MongoDB, Consul, and feature management.

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
     - `Events/ExampleCreatedEvent.cs`: Defines an event for event sourcing.

6. **IdentityServer**
   - **Purpose**: Manages authentication and authorization using IdentityServer4.
   - **Key Files**:
     - `Config.cs`: Configures IdentityServer clients, resources, and test users.
     - `Program.cs`: Configures IdentityServer.

## Technologies Used

- **gRPC**: For high-performance, language-agnostic RPC communication.
- **RabbitMQ**: For asynchronous messaging between services.
- **SignalR**: For real-time notifications.
- **GraphQL**: For flexible and efficient data querying.
- **Ocelot**: For API Gateway functionality.
- **Entity Framework Core**: For SQL Server database access.
- **MongoDB**: For NoSQL database access.
- **JWT Authentication**: For secure user authentication.
- **Swagger**: For API documentation.
- **IdentityServer4**: For OAuth2 and OpenID Connect.
- **Consul**: For configuration management.
- **Feature Management**: For conditional feature deployment using Microsoft.FeatureManagement.

## Setup and Usage

### Prerequisites

- .NET 8 SDK
- Docker
- Kubernetes (kubectl and Helm)
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
       "DefaultConnection": "Server=your_server;Database=GrpcServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true",
       "MongoDb": "mongodb://mongodb:27017"
     },
     "Consul": {
       "Address": "http://localhost:8500",
       "ServiceAddress": "localhost",
       "ServicePort": "5000"
     },
     "IdentityServer": {
       "Authority": "http://identityserver-service.default.svc.cluster.local",
       "ClientId": "client",
       "ClientSecret": "secret",
       "Scope": "grpcservice"
     },
     "FeatureManagement": {
       "NewFeature": {
         "EnabledFor": [
           {
             "Name": "Percentage",
             "Parameters": {
               "Value": 50
             }
           }
         ]
       }
     }
   }
   ```

4. **Build the solution**:
   Open the solution in Visual Studio and build it to restore all dependencies.

### Docker Setup

1. **Build Docker Images**:
   - **ApiGateway**:
     ```sh
     docker build -t apigateway -f ApiGateway/Dockerfile .
     ```
   - **AuthService**:
     ```sh
     docker build -t authservice -f AuthService/Dockerfile .
     ```
   - **GrpcService**:
     ```sh
     docker build -t grpcservice -f GrpcService/Dockerfile .
     ```
   - **RabbitMqWorker**:
     ```sh
     docker build -t rabbitmqworker -f RabbitMqWorker/Dockerfile .
     ```
   - **IdentityServer**:
     ```sh
     docker build -t identityserver -f IdentityServer/Dockerfile .
     ```

2. **Run Docker Containers**:
   - **ApiGateway**:
     ```sh
     docker run -d -p 5000:80 --name apigateway apigateway
     ```
   - **AuthService**:
     ```sh
     docker run -d -p 5002:80 --name authservice authservice
     ```
   - **GrpcService**:
     ```sh
     docker run -d -p 5001:80 --name grpcservice grpcservice
     ```
   - **RabbitMqWorker**:
     ```sh
     docker run -d --name rabbitmqworker rabbitmqworker
     ```
   - **IdentityServer**:
     ```sh
     docker run -d -p 5003:80 --name identityserver identityserver
     ```

### Kubernetes Setup

1. **Deploy Consul**:
   - Add the HashiCorp Helm repository and deploy Consul.
     ```sh
     helm repo add hashicorp https://helm.releases.hashicorp.com
     helm repo update
     helm install consul hashicorp/consul --values k8s/consul/values.yaml
     ```

2. **Apply Kubernetes Configurations**:
   - **ApiGateway**:
     ```sh
     kubectl apply -f k8s/apigateway/apigateway-deployment.yml
     ```
   - **AuthService**:
     ```sh
     kubectl apply -f k8s/authservice/authservice-deployment.yml
     ```
   - **GrpcService**:
     ```sh
     kubectl apply -f k8s/grpcservice/grpcservice-deployment.yml
     ```
   - **RabbitMqWorker**:
     ```sh
     kubectl apply -f k8s/rabbitmqworker/rabbitmqworker-deployment.yml
     ```
   - **IdentityServer**:
     ```sh
     kubectl apply -f k8s/identityserver/identityserver-deployment.yml
     ```

3. **Update Istio Ingress Gateway**:
   - Apply the Istio ingress gateway configuration.
     ```sh
     kubectl apply -f k8s/istio-ingressgateway.yml
     ```

### Running the Services

1. **ApiGateway**:
   - Access the API Gateway at `http://localhost:5000`.

2. **AuthService**:
   - Access Swagger UI at `http://localhost:5002/swagger` to explore authentication endpoints.
   - Access GraphQL endpoint at `http://localhost:5002/graphql`.

3. **GrpcService**:
   - The gRPC service will be available at `http://localhost:5001`.

4. **RabbitMqWorker**:
   - The background worker processes messages from RabbitMQ.

5. **IdentityServer**:
   - Access IdentityServer at `http://localhost:5003`.

### How It Works

- **ApiGateway**: Routes requests to the appropriate services based on `ocelot.json` configuration.
- **AuthService**:
  - Provides JWT authentication endpoints (`/Auth/token`).
  - Exposes a GraphQL endpoint (`/graphql`) to query user data.
- **GrpcService**:
  - Implements a gRPC service (`GreeterService`) with methods defined in `greet.proto

`.
  - Publishes and consumes messages from RabbitMQ.
  - Sends real-time notifications to connected clients via SignalR.
  - Uses feature flags to enable or disable features.
- **RabbitMqWorker**: Listens to RabbitMQ messages and processes them, demonstrating background task processing.
- **IdentityServer**: Manages authentication and authorization using OAuth2 and OpenID Connect.

### Example Requests

1. **Authenticate and Get JWT Token**:
   - Send a POST request to `http://localhost:5002/Auth/token` with the following JSON body:
     ```json
     {
       "Username": "test",
       "Password": "password"
     }
     ```

2. **Call gRPC Service**:
   - Use a gRPC client tool like [grpcurl](https://github.com/fullstorydev/grpcurl) to call the `SayHello` method on the `GrpcService`.

3. **Subscribe to SignalR Notifications**:
   - Connect to `http://localhost:5001/hubs/notification` using a SignalR client to receive real-time messages.

4. **Query Data via GraphQL**:
   - Send a POST request to `http://localhost:5002/graphql` with the following GraphQL query:
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
