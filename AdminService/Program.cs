using System;
using System.Runtime;
using AdminService.Insfrastructure;
using Microsoft.EntityFrameworkCore;
using Utils;

var builder = WebApplication.CreateBuilder(args);
// Dùng ConfigHelper để lấy chuỗi kết nối
var config = CommonUtils.GetConfiguration();
var connectionString = config.GetConnectionString("PostgreSQLDatabase");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

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
