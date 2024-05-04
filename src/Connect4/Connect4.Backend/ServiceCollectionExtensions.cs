﻿using Game.Application;
using Microsoft.Extensions.DependencyInjection;
using Visualizer.Application;

namespace Connect4.Backend
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackend(this IServiceCollection services)
        {
            services.AddGame();
            services.AddVisualizer();

            return services;
        }
    }
}
