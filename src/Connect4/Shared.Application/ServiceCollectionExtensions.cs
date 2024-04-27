using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedApplication(this IServiceCollection services)
        {
            services.AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));

            return services;
        }
    }
}
