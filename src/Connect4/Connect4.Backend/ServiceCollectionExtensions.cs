using Game.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Connect4.Backend
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackend(this IServiceCollection services)
        {
            services.AddGame();

            return services;
        }
    }
}
