using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ZooKeeper.Infrastructure.InMemory;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Application.Services;
using ZooKeeper.Infrastructure.EventBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация сервисов
builder.Services.AddScoped<AnimalTransferService>();
builder.Services.AddScoped<FeedingOrganizationService>();
builder.Services.AddScoped<ZooStatisticsService>();
builder.Services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();

// Регистрация репозиториев
builder.Services.AddSingleton<IAnimalRepository, AnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
            new BadRequestObjectResult(context.ModelState);
    });
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();