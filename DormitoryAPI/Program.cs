using Microsoft.EntityFrameworkCore;

using Dormitory.Models.Entities;
using Dormitory.DAO.Implementations;
using Dormitory.DAO.Interfaces;
using Dormitory.BUS.Implementations;
using Dormitory.BUS.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DormitoryContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("QL_KXT")));

var app = builder.Build();
app.UseSwagger();
    app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

// --- Bắt đầu đoạn code thêm ---
// Tự động tạo bảng Database khi khởi động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Lưu ý: Đổi 'DormitoryContext' thành tên đúng của DbContext trong code bạn
        var context = services.GetRequiredService<DormitoryContext>();
        
        // Lệnh này sẽ tự động tạo bảng nếu chưa có
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi khởi tạo Database.");
    }
}
// --- Kết thúc đoạn code thêm ---

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
