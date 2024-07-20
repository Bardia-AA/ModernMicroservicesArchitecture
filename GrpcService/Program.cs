using Consul;
using GrpcService.Data;
using GrpcService.Hubs;
using GrpcService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Swashbuckle.AspNetCore.SwaggerGen;
using GrpcService.Services.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<CreateExampleCommandHandler>();
builder.Services.AddDbContext<GrpcServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GrpcService API", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "GrpcService API", Version = "v2" });
    c.DocInclusionPredicate((version, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;
        var versions = methodInfo.DeclaringType
            .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions);
        return versions.Any(v => $"v{v}" == version);
    });
});

// Configure Consul
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    cfg.Address = new Uri(builder.Configuration["Consul:Address"]);
}));

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://identityserver-service.default.svc.cluster.local";
        options.Audience = "grpcservice";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

// Add feature management
builder.Services.AddFeatureManagement()
                .AddFeatureFilter<PercentageFilter>();

var app = builder.Build();

// Register with Consul
var lifetime = app.Lifetime;
var consulClient = app.Services.GetRequiredService<IConsulClient>();

var registration = new AgentServiceRegistration()
{
    ID = "grpcservice", // unique ID of the service
    Name = "grpcservice", // name of the service
    Address = builder.Configuration["Consul:ServiceAddress"], // the address of the service
    Port = int.Parse(builder.Configuration["Consul:ServicePort"]) // the port of the service
};

consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrpcService API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "GrpcService API v2");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<GreeterService>();
app.MapHub<NotificationHub>("/hubs/notification");
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
});

app.Run();