using FunctionalTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Score.Clients.Clients;

namespace FunctionalTests.Setup.IoC
{
    public static class MockClientRegistrator
    {
        public static IServiceCollection RegisterMockClients(this IServiceCollection services)
        {
            services.AddSingleton<IScoreClient, MockScoreClient>();

            return services;
        }
    }
}
