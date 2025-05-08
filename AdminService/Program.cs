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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Utils;
using Utils.Middlewares;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Utils.Cache;
using AdminService.Business.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
// Cấu hình In-Memory Cache
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Địa chỉ Redis server
    options.InstanceName = "MyApp_"; // Tiền tố cho các khóa cache
});

// Đăng ký các dịch vụ Cache
builder.Services.AddScoped<InMemoryCacheService>();
builder.Services.AddScoped<RedisCacheService>();
builder.Services.AddScoped<CacheService>();

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

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Cấu hình Authorization (nếu bạn sử dụng roles)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "your-redis-server:6379,password=your-password";
    options.InstanceName = "MyApp_";
});

var app = builder.Build();

// Dịch vụ API dùng chung để test cache
app.MapGet("/cache", async (CacheService cacheService) =>
{
    string key = "myCacheKey";
    string cachedData = await cacheService.GetCacheAsync(key);

    if (string.IsNullOrEmpty(cachedData))
    {
        cachedData = "This is some fetched data.";
        await cacheService.SetCacheAsync(key, cachedData);
    }

    return Results.Ok(cachedData);
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Luôn gọi UseCors TRƯỚC khi authorization
//CORS(Cross - Origin Resource Sharing) cho phép một web app (frontend) truy cập tài nguyên từ một domain khác (backend).

//Ví dụ:
//Frontend tại http://localhost:3000 gọi API từ https://api.mysite.com → cần bật CORS.

//Nếu không bật → trình duyệt sẽ chặn request vì lý do bảo mật.
app.UseCors("AllowFrontend");

app.UseAuthentication(); // Thêm vào để sử dụng xác thực
app.UseAuthorization();
// Đăng ký middleware vào pipeline
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Ví dụ các middleware có sẵn

//app.UseRouting();
//app.UseDeveloperExceptionPage();
//app.UseExceptionHandler();
//app.UseStaticFiles();

// Đăng ký custom middleware
app.UseRequestTiming();
app.UseGlobalExceptionHandler();
app.MapControllers();

app.Run();
