using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Scores.Infrastructure.MediatR.Behaviours;

namespace Scores.Infrastructure.IoC
{
    public static class MediatRRegistrator
    {
        public static IServiceCollection RegisterMediatR(this IServiceCollection services)
        {
            services.AddMediatR(GetAssembliesUsingMediatR());

            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            return services;
        }

        private static Assembly[] GetAssembliesUsingMediatR()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith(Constants.Namespace))
                .ToArray();

            return assemblies;
        }
    }
}
