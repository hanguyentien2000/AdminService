using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using AdminService.Business.Jwt;
using AdminService.Business.Jwt.Token;
using AdminService.Business.Roles;
using AdminService.Business.User;
using AdminService.Insfrastructure;
using DataUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Utils;

var builder = WebApplication.CreateBuilder(args);
// Dùng ConfigHelper để lấy chuỗi kết nối
//var config = CommonUtils.GetConfiguration();
//var connectionString = config.GetConnectionString("MSSQLDatabase");
// Add services to the container.
builder.Services.AddDbContext<AdminDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String: {connectionString}");  
builder.Services.AddScoped<IUserHandler>(provider =>
{
    var context = provider.GetRequiredService<AdminDataContext>();
    var factory = new DatabaseFactory(context);
    return new UserHandler(factory); // Constructor này cần overload nếu không dùng options
});
builder.Services.AddScoped<IRoleHandler>(provider =>
{
    var context = provider.GetRequiredService<AdminDataContext>();
    var factory = new DatabaseFactory(context);
    return new RoleHandler(factory); // Constructor này cần overload nếu không dùng options
});
builder.Services.AddScoped<TokenService>();

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
