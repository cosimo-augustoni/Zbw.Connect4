using Game.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGameInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
