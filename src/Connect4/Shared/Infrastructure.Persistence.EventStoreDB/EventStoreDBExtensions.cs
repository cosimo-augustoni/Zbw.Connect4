using Domain;
using Infrastructure.Persistence.EventStoreDB.EventRegistration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure.Persistence.EventStoreDB
{
    public static class EventStoreDBExtensions
    {
        public static IServiceCollection AddEventSourcedRepository<TAggregateRoot, TAggregateRootFactory>(this IServiceCollection services)
            where TAggregateRoot : IAggregateRoot<TAggregateRoot>
            where TAggregateRootFactory : class, IAggregateRootFactory<TAggregateRoot>
        {
            return services
                .AddSingleton<IAggregateRootFactory<TAggregateRoot>, TAggregateRootFactory>()
                .AddScoped<IRepository<TAggregateRoot>, EventSourcedRepository<TAggregateRoot>>()
                .AddScoped<IReadOnlyRepository<TAggregateRoot>>(s => s.GetRequiredService<IRepository<TAggregateRoot>>())
                .AddTransient<IEventTransformer<TAggregateRoot>, EventTransformer<TAggregateRoot>>();
        }

        public static IServiceCollection RegisterEvent<TEvent, TAggregateRoot>(this IServiceCollection services, string? eventType = null)
            where TEvent : DomainEvent<TAggregateRoot>
            where TAggregateRoot : IAggregateRoot<TAggregateRoot>
        {
            var eventRegistration = ServiceDescriptor.Singleton<IEventRegistration<TAggregateRoot>>(_ =>
                new EventRegistration<TAggregateRoot, TEvent>(eventType ?? typeof(TEvent).Name));

            services.TryAddEnumerable(eventRegistration);

            return services;
        }
    }
}
