﻿using AbstractedRabbitMQ.Publishers;
using AbstractedRabbitMQ.Subscribers;
using Microsoft.Extensions.DependencyInjection;

namespace AbstractedRabbitMQ.Setup
{
    public static class ConfigureService
    {
        public static IServiceCollection AddRabbitMQConnection(this IServiceCollection services, Action<ConnectionConfig> options)
        {
            var config = new ConnectionConfig();
            options.Invoke(config);
            services.AddSingleton<IConnectionProvider>(x => new ConnectionProvider(config));

            return services;
        }

        public static IServiceCollection AddRabbitMQPublisher(this IServiceCollection services, Action<PublisherConfig> options)
        {
            var config = new PublisherConfig();
            options.Invoke(config);
            services.AddSingleton<IPublisher>(x => new Publisher(x.GetRequiredService<IConnectionProvider>(),config));
            return services;
        }
        public static IServiceCollection AddRabbitMQSubscriber(this IServiceCollection services, Action<SubScribeConfig> options)
        {
            var config=new SubScribeConfig();
            options.Invoke(config);
            services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetRequiredService<IConnectionProvider>(),config));
            return services;
        }
    }
}
