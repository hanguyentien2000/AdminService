using EventBusRabbitMqueue;
using Microsoft.AspNetCore.Authentication.BearerToken;
using PaymentService.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình RabbitMQService
builder.Services.AddSingleton<IRabbitMqueueHandler>(sp =>
{
    return new RabbitMqueueHandler("localhost"); // Địa chỉ của RabbitMQ server
});

// Add services to the container.

// Đăng ký ConsumerController như một background service
builder.Services.AddHostedService<ConsumerController>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
