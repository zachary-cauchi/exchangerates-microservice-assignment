using Serilog;
using Serilog.Core;
using System.Configuration;

namespace Exchange.API.Extensions
{
    public static class LoggingExtensions
    {
        public static WebApplicationBuilder ConfigureExchangeLogging(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];

            Serilog.Debugging.SelfLog.Enable(Console.Out);

            builder.Logging.AddSerilog(new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithProperty("ApplicationContext", "Exchange.Api")
                .Enrich.FromLogContext()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .WriteTo.Console()
                .CreateLogger());

            builder.Host.UseSerilog();

            return builder;
        }
    }
}
