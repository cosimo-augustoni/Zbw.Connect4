using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using Visualizer.Domain.VisualizerAggregate;
using Visualizer.Domain.VisualizerProjections;
using Visualizer.Infrastructure.VisualizerAggregate;
using Visualizer.Infrastructure.VisualizerProjections;
using Visualizer.Infrastructure.VisualizerProjections.Detail;
using Visualizer.Infrastructure.VisualizerProjections.Summary;

namespace Visualizer.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVisualizerInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();

            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddScoped<IVisualizerRepository, VisualizerRepository>();
            services.AddScoped<IVisualizerCollectionProvider, VisualizerCollectionProvider>();
            services.AddScoped<IVisualizerDetailQuery, VisualizerDetailQuery>();
            services.AddScoped<IVisualizersQuery, VisualizersQuery>();

            return services;
        }
    }
}
