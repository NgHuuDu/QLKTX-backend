using AutoMapper;
using DormitoryManagementSystem.BUS.Implementations;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DAO.Context;
using DormitoryManagementSystem.DAO.Implementations;
using DormitoryManagementSystem.DAO.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DormitoryManagementSystem.API", Version = "v1" });

    // --- PHẦN BỔ SUNG ĐỂ HIỆN NÚT AUTHORIZE ---
    
    // Định nghĩa loại bảo mật là "Bearer" (JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Yêu cầu Swagger sử dụng định nghĩa trên cho các API
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// --- 2. CẤU HÌNH JWT AUTHENTICATION ---
// Lấy Key từ appsettings.json 
var secretKey = builder.Configuration["Jwt:Key"] ?? "DayLaCaiKeyBiMatCuaNhomChungToiKhongDuocTietLo123456"; 

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Kiểm tra chữ ký (Quan trọng nhất)
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

        // Tạm thời tắt check Issuer/Audience để tránh lỗi cấu hình domain khi deploy Render
        // (Nếu muốn bảo mật cao hơn thì bật lên true và điền đúng domain vào appsettings)
        ValidateIssuer = false, 
        ValidateAudience = false,
        
        // Kiểm tra thời gian hết hạn (Token expired)
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");
}
builder.Services.AddDbContext<PostgreDbContext>(options =>
    options.UseNpgsql(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(typeof(DormitoryManagementSystem.Mappings.MappingProfile));

// DAOs
builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<IRoomDAO, RoomDAO>();
builder.Services.AddScoped<IBuildingDAO, BuildingDAO>();
builder.Services.AddScoped<IContractDAO, ContractDAO>();
builder.Services.AddScoped<IStudentDAO, StudentDAO>();
builder.Services.AddScoped<INewsDAO, NewsDAO>();
builder.Services.AddScoped<IPaymentDAO, PaymentDAO>();
builder.Services.AddScoped<IViolationDAO, ViolationDAO>();
builder.Services.AddScoped<IAdminDAO, AdminDAO>();
builder.Services.AddScoped<IStatisticsDAO, StatisticsDAO>();

// BUSs
builder.Services.AddScoped<IUserBUS, UserBUS>();
builder.Services.AddScoped<IRoomBUS, RoomBUS>();
builder.Services.AddScoped<IBuildingBUS, BuildingBUS>();
builder.Services.AddScoped<IContractBUS, ContractBUS>();
builder.Services.AddScoped<IStudentBUS, StudentBUS>();
builder.Services.AddScoped<INewsBUS, NewsBUS>();
builder.Services.AddScoped<IPaymentBUS, PaymentBUS>();
builder.Services.AddScoped<IViolationBUS, ViolationBUS>();
builder.Services.AddScoped<IAdminBUS, AdminBUS>();
builder.Services.AddScoped<IStatisticsBUS, StatisticsBUS>();

var app = builder.Build();

// Cấu hình pipeline xử lý HTTP request
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
