using Microsoft.Extensions.DependencyInjection;
using PlayerClient.Infrastructure;
using PlayerClient.Local;
using Shared.Application;

namespace PlayerClient.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlayerClients(this IServiceCollection services)
        {
            services.AddSharedApplication();
            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddLocalPlayerClient();

            services.AddPlayerClientInfrastructure();

            return services;
        }
    }
}
