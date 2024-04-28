using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEventPublisher, EventPublisher>();

            return services;
        }
    }
}
