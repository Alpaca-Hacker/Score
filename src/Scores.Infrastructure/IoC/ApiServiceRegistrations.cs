

using Microsoft.Extensions.DependencyInjection;
using Score.Domain.Services;

namespace Scores.Infrastructure.IoC
{
    public static class ApiServiceRegistrations
    {
        public static IServiceCollection RegisterApiDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IScoreService, ScoreService>();

            return services;
        }
    }
}
