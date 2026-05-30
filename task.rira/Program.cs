using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;
using System.Globalization;
using task.rira.Abstractions;
using task.rira.Data;
using task.rira.Implementations;
using task.rira.sdk;
using task.rira.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=rira");
});

builder.Services.AddLogging(a => a.AddConsole());

builder.Services.AddCodeFirstGrpc(a => { });

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

builder.Services.AddSingleton<PersonCache>();
builder.Services.AddSingleton<IQueue, QueueService>();
builder.Services.AddScoped<IPersonRepo, PersonRepo>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHostedService<PersonConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<PersonService>();

using var scope = app.Services.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

db.Database.EnsureCreated();

app.Run();
