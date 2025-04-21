using System;
using System.Runtime;
using AdminService.Business.User;
using AdminService.Insfrastructure;
using DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Utils;

var builder = WebApplication.CreateBuilder(args);
// Dùng ConfigHelper để lấy chuỗi kết nối
var config = CommonUtils.GetConfiguration();
var connectionString = config.GetConnectionString("PostgreSQLDatabase");

builder.Services.AddDbContext<AdminDataContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserHandler>(provider =>
{
    var context = provider.GetRequiredService<AdminDataContext>();
    var factory = new DatabaseFactory(context);
    return new UserHandler(factory); // Constructor này cần overload nếu không dùng options
});

// Add services to the container.

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
