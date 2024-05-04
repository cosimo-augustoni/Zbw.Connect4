using Microsoft.Extensions.DependencyInjection;
using Shared.Application;
using Visualizer.Infrastructure;

namespace Visualizer.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVisualizer(this IServiceCollection services)
        {
            services.AddSharedApplication();
            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddVisualizerInfrastructure();

            return services;
        }
    }
}
