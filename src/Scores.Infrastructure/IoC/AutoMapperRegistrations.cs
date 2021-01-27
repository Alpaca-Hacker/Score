using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Scores.Infrastructure.IoC
{
    public static class AutoMapperRegistrations
    {
        public static IServiceCollection RegisterAutoMapperAssemblies(this IServiceCollection services)
        {
            services.AddAutoMapper(GetAssemblies());
            return services;
        }

        private static Assembly[] GetAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith(Constants.Namespace))
                .ToArray();

            return assemblies;
        }
    }
}
