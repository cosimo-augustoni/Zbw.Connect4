using Microsoft.Extensions.DependencyInjection;
using PlayerClient.Contract;
using PlayerClient.Domain;

namespace PlayerClient.Local
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalPlayerClient(this IServiceCollection services)
        {
            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddTransient<IPlayerClientConnector, LocalPlayerClientConnector>();
            services.AddTransient<IPlayerClientFactory, LocalPlayerClientFactory>();

            return services;
        }
    }
}
