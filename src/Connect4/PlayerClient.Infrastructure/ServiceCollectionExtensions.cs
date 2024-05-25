using Microsoft.Extensions.DependencyInjection;
using PlayerClient.Domain;
using Shared.Infrastructure;

namespace PlayerClient.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlayerClientInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();

            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            services.AddTransient<IPlayerAssignmentQuery, PlayerAssignmentViewQuery>();

            return services;
        }
    }
}
