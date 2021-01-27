using Microsoft.Extensions.DependencyInjection;
using Score.Clients.Clients;

namespace Scores.Infrastructure.IoC
{
    public static class ClientRegistrations
    {
        public static IServiceCollection RegisterApiClientDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IScoreClient, ScoreClient>();

            return services;
        }

    }
}
