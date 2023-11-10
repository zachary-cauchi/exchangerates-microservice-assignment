using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Exchange.API.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            static void ConfigureSqlOptions(SqlServerDbContextOptionsBuilder sqlOptions)
            {
                sqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);

                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            }

            services.AddDbContext<ExchangeAPIContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("ExchangeAPIContext");

                options.UseSqlServer(connectionString, ConfigureSqlOptions);
            });

            return services;
        }
    }
}
