using Game.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application;

namespace Game.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame(this IServiceCollection services)
        {
            services.AddSharedApplication();
            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddGameInfrastructure();

            return services;
        }
    }
}
