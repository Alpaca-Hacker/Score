using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Scores.Infrastructure.IoC
{
    public static class ConfigurationRegistrator
    {
        public static IServiceCollection RegisterConfiguration(this IServiceCollection services)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            services.AddSingleton<IConfiguration>(provider =>
            {
                var configProvider = new ConfigurationBuilder()
                    .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                    .Build();

                return configProvider;
            });

            return services;
        }
    }
}
