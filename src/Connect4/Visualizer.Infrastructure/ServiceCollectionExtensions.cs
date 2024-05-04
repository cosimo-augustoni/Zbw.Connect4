using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using Visualizer.Domain.VisualizerAggregate;
using Visualizer.Domain.VisualizerProjections;
using Visualizer.Infrastructure.VisualizerAggregate;
using Visualizer.Infrastructure.VisualizerProjections;
using Visualizer.Infrastructure.VisualizerProjections.Detail;
using Visualizer.Infrastructure.VisualizerProjections.Summary;
using Visualizer.Physical.Infrastructure;

namespace Visualizer.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVisualizerInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();
            services.AddVisualizerPhysicalInfrastructure();

            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddTransient<IVisualizerRepository, VisualizerRepository>();
            services.AddTransient<IVisualizerCollectionProvider, VisualizerCollectionProvider>();
            services.AddTransient<IVisualizerDetailQuery, VisualizerDetailQuery>();
            services.AddTransient<IVisualizersQuery, VisualizersQuery>();

            return services;
        }
    }
}
