using dotenv.net;
using LendLoopAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;
using dotenv.net;


var builder = WebApplication.CreateBuilder(args);
DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { "../.env.local" }, ignoreExceptions: false));
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LendLoopContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); 


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
