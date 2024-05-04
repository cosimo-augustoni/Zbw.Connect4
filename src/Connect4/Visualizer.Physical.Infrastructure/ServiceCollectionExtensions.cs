using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;

namespace Visualizer.Physical.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVisualizerPhysicalInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();

            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddHostedService<VisualizerClientSubscriptionHandler>();
            services.AddSingleton<IVisualizerMqttClient, VisualizerMqttClient>();
            services.AddSingleton<IVisualizerStatusWatcher, VisualizerStatusWatcher>();

            return services;
        }
    }
}
