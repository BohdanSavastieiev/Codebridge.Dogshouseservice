using Codebridge.TechnicalTask.API.Common.Extensions.Configuration;
using Codebridge.TechnicalTask.Application;
using Codebridge.TechnicalTask.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()); 

builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware();
await app.ConfigureEndpoints();

app.Run();

public partial class Program { }