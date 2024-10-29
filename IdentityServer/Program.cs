using IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddTestUsers(Config.TestUsers)
    .AddDeveloperSigningCredential();

// Register IUrlHelper for return URL validation
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
builder.Services.AddSingleton<IUrlHelper>(provider =>
{
    var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
    var actionContext = new ActionContext
    {
        HttpContext = httpContextAccessor?.HttpContext,
    };
    return new UrlHelper(actionContext);
});

var app = builder.Build();

app.UseIdentityServer();

// Middleware to validate return URLs
app.Use(async (context, next) =>
{
    var urlHelper = context.RequestServices.GetRequiredService<IUrlHelper>();
    if (context.Request.Query.TryGetValue("returnUrl", out var returnUrl))
    {
        if (!urlHelper.IsLocalUrl(returnUrl))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Invalid return URL");
            return;
        }
    }
    await next();
});

app.Run();