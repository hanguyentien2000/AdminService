using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using AdminService.Business.Jwt;
using AdminService.Business.Jwt.Token;
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

builder.Services.Configure<JwtModel>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<IJwtHandler, JwtHandler>();
builder.Services.AddSingleton<ITokenStoreHandler, TokenStoreHandler>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtModel>()!;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };

    // Custom token validation with token store
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var token = context.SecurityToken as JwtSecurityToken;
            var tokenStore = context.HttpContext.RequestServices.GetRequiredService<ITokenStoreHandler>();
            if (userId == null || token == null || !tokenStore.IsTokenValid(userId, token.RawData))
            {
                context.Fail("Invalid token");
            }
            return Task.CompletedTask;
        }
    };
});

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
