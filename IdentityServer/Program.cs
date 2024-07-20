using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    })
    .AddInMemoryApiScopes(new List<ApiScope>
    {
        new ApiScope("grpcservice", "gRPC Service")
    })
    .AddInMemoryClients(new List<Client>
    {
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
    })
    .AddTestUsers(new List<TestUser>
    {
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
    })
    .AddDeveloperSigningCredential();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseIdentityServer();

app.Run();