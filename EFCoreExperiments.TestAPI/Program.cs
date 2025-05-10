using Microsoft.OpenApi.Models;
using Serilog;
using EFCoreExperiments.DataContext;
using EFCoreExperiments.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment)
        .Enrich.WithProperty("ApplicationLevel", hostingContext.HostingEnvironment.EnvironmentName);
});

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
});

builder.Services.UseContexts(builder.Configuration);
builder.Services.UseCore(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
