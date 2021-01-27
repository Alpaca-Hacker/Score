using FunctionalTests.Setup.IoC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Score.Api;
using Scores.Infrastructure.IoC;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FunctionalTests.Setup
{
    public class TestHttpServerBuilder : WebHostBuilder
    {
        public TestHttpServerBuilder()
        {
            this.UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services
                        .RegisterApiDependencies()
                        .RegisterMediatR()
                        .RegisterApiClientDependencies()
                        .RegisterAutoMapperAssemblies()
                        .RegisterMockClients();

                    services.AddSingleton(provider => new LoggerConfiguration().ReadFrom.Configuration(provider.GetService<IConfiguration>())
                        .WriteTo.TestCorrelator());

                    services.AddMvcCore();

                    services.AddSingleton<ILogger>(
                        provider => provider.GetService<LoggerConfiguration>().CreateLogger());
                    services.AddSingleton<ILoggerFactory>(provider =>
                        new Serilog.Extensions.Logging.SerilogLoggerFactory(provider.GetService<ILogger>()));
                });
        }

        public new TestServer Build() => new TestServer(this);
    }
}
