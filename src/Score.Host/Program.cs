
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Score.Api;
using Scores.Infrastructure;
using Scores.Infrastructure.IoC;

namespace Score.Host
{
    class Program
    {
        private const int ServicePort = 6650;

        public static Task Main(string[] args)
        {
            IWebHost host = CreateWebHost(args);

            return host.RunAsync();
        }

        private static IWebHost CreateWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options => { options.Listen(IPAddress.Any, ServicePort); })
                .ConfigureServices(services =>
                {
                    services.RegisterConfiguration()
                        .RegisterDefaultLogging("Scores.Api")
                        .RegisterApiDependencies()
                        .RegisterApiClientDependencies()
                        .RegisterAutoMapperAssemblies()
                        .RegisterMediatR();

                })
                .Build();
        }
    }
}
