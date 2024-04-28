using Game.Domain;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;

namespace Game.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGameInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();

            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
