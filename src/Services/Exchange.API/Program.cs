using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Exchange.API.Data;
using Exchange.API.Extensions;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContexts(builder.Configuration);

builder.ConfigureExchangeLogging(builder.Configuration);


var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ExchangeAPIContext>();
        await context.Database.MigrateAsync();
        await new ExchangeAPIContextSeed().ClearAndSeedAsync(context);
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
