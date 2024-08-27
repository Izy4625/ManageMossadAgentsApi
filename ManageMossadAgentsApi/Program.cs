using ManageMossadAgentsApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ManageMossadAgentsApi.Controllers;
using ManageMossadAgentsApi.Services;
using ManageMossadAgentsApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TargetHandler, TargetHandler>();
builder.Services.AddScoped<AgentHandler, AgentHandler>();
builder.Services.AddScoped<MissionHandler, MissionHandler>();

string? ConnectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MossadDbContext>(option => option.UseSqlServer(ConnectionStrings));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
//app.UseWhen(
//    context =>
//        context.Request.Path.StartsWithSegments("agents"),
//    appBuilder =>
//    {
//        appBuilder.UseMiddleware<JwtValidationMiddleware>();
//        //appBuilder.UseMiddleware<JwtValidationMiddleware>();
//    });

app.MapControllers();



app.Run();
