
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Scores.Infrastructure.IoC
{
    public static class DefaultLoggingRegistrator
    {
        public static IServiceCollection RegisterDefaultLogging(this IServiceCollection services,
            string applicationName)
        {
            services.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();

                var serilogConfiguration = new SerilogConfigurationBuilder()
                    .WithApplicationName(applicationName)
                    .WithConsoleOutput()
                    .WithEventsFromLogContext()
                    .WithMachineName();

                LoggerConfiguration config = serilogConfiguration.Build();

                config.ReadFrom.Configuration(configuration);

                return config;
            });

            services.AddSingleton(provider =>
            {
                // Also store this as the global Serilog instance, so that it can be used statically
                Log.Logger = provider.GetService<LoggerConfiguration>().CreateLogger();
                return Log.Logger;
            });

            services.AddSingleton<ILoggerFactory>(provider =>
                new Serilog.Extensions.Logging.SerilogLoggerFactory(provider.GetService<ILogger>()));

            return services;
        }
    }
}
