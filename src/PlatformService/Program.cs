using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Data.Repositories;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => 
{    
    opt.UseInMemoryDatabase("InMem");
});

builder.Services.AddScoped<IRepository<Platform>, Repository<Platform>>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddScoped<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

PreparationDb.PrepareDb(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
