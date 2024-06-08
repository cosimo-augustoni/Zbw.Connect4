using Microsoft.Extensions.DependencyInjection;
using PlayerClient.Contract;
using PlayerClient.Domain;

namespace PlayerClient.AI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAiPlayerClient(this IServiceCollection services)
        {
            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddTransient<IPlayerClientConnector, EasyAiPlayerClientConnector>();
            services.AddTransient<IPlayerClientConnector, MediumAiPlayerClientConnector>();
            services.AddTransient<IPlayerClientConnector, HardAiPlayerClientConnector>();
            services.AddTransient<IPlayerClientFactory, AiPlayerClientFactory>();

            return services;
        }
    }
}
