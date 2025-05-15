using EventBusRabbitMqueue.Abstractions;
using EventBusRabbitMqueue.Logging;
using EventBusRabbitMqueue.Middleware;
using PaymentService.Business.Core;
using PaymentService.Intergrate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 📌 Đăng ký controller
builder.Services.AddControllers();
//builder.Services.AddSingleton<IErrorLogger, MongoErrorLogger>();

builder.Services.AddSingleton<IErrorLogger, MongoErrorLogger>();

builder.Services.AddScoped<IEventHandler<UserCreatedEvent>, UserCreatedHandler>();
builder.Services.AddScoped<IMessageMiddleware<UserCreatedEvent>, LoggingMiddleware<UserCreatedEvent>>();

builder.Services.AddHostedService<UserCreatedConsumer>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.Authority = "https://localhost:5110";
//        options.Audience = "payment-api";
//    });

builder.Services.AddAuthorization(); // 

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
