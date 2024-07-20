using IdentityServer4.Models;
using IdentityServer4.Test;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    ])
    .AddInMemoryApiScopes(
    [
        new ApiScope("grpcservice", "gRPC Service")
    ])
    .AddInMemoryClients(
    [
        new Client
        {
            ClientId = "client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            AllowedScopes = { "grpcservice" }
        }
    ])
    .AddTestUsers(
    [
        new TestUser
        {
            SubjectId = "1",
            Username = "alice",
            Password = "password"
        },
        new TestUser
        {
            SubjectId = "2",
            Username = "bob",
            Password = "password"
        }
    ])
    .AddDeveloperSigningCredential();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseIdentityServer();

app.Run();