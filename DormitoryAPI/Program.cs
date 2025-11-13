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
