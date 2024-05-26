using Game.Domain.GameAggregate;
using Game.Domain.GameProjections;
using Game.Infrastructure.GameAggregate;
using Game.Infrastructure.GameProjections.Games;
using Game.Infrastructure.GameProjections.Lobbies;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;

namespace Game.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGameInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();

            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IGameLobbiesQuery, GameLobbiesQuery>();
            services.AddTransient<IGameQuery, GameQuery>();

            return services;
        }
    }
}
