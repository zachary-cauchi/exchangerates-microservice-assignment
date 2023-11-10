using Exchange.API.Validations;
using FluentValidation;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Add DbContexts.

services.AddDbContexts(builder.Configuration);

// Add repositories.

services.AddScoped<ICurrencyRepository, CurrencyRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IAccountBalanceRepository, AccountBalanceRepository>();
services.AddScoped<IPastTransactionRepository, PastTransactionRepository>();

// Add services.

services.AddScoped<ICurrencyService, CurrencyService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IAccountBalanceService, AccountBalanceService>();
services.AddScoped<IPastTransactionService, PastTransactionService>();
services.AddScoped<IExchangeRateFixerService, ExchangeRateFixerService>();

// Configure logging.

builder.ConfigureExchangeLogging(builder.Configuration);

// Configure MediatR

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("ConnectionStrings:Redis");
    options.InstanceName = "Exchange_API_Cache_";
});

// Configure command validators.

services.AddSingleton<IValidator<CreateCurrencyExchangeCommand>, CreateCurrencyExchangeCommandValidator>();

// Add an HTTP client for calling the exchange rates provider.

string apiKey = builder.Configuration.GetValue<string>("ExchangeRateProviders:Fixer:ApiKey") ?? throw new ArgumentNullException("No Fixer API key was found. Cannot call API.");
string connectionString = builder.Configuration.GetValue<string>("ExchangeRateProviders:Fixer:BaseAddress") ?? "https://api.exchangeratesapi.io/v1/latest";

builder.Services.AddHttpClient<IExchangeRateFixerService, ExchangeRateFixerService>(client =>
{
    string connectionString = builder.Configuration.GetValue<string>("ExchangeRateProviders:Fixer:BaseAddress") ?? "https://api.exchangeratesapi.io/v1/";
    client.BaseAddress = new Uri(connectionString);
}).SetHandlerLifetime(TimeSpan.FromMinutes(5));

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
