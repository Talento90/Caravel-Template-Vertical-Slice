﻿using System.Reflection;
using MassTransit;

namespace CaravelTemplate.Api.Shared.Messaging;

public static class MassTransitExtensions
{
    public static void RegisterMassTransit(this IServiceCollection services, MassTransitOptions options)
    {
        services.AddMassTransit((busConfig) =>
        {
            // Endpoints where we publish messages
            busConfig.SetKebabCaseEndpointNameFormatter();

            // Register all consumers via assembly scan
            busConfig.AddConsumers(Assembly.GetExecutingAssembly());

            busConfig.UsingRabbitMq((context, config) =>
            {
                config.Host(options.Host, options.Port, options.VirtualHost, host =>
                {
                    host.Username(options.Username);
                    host.Password(options.Password);
                });

                config.ConfigureEndpoints(context);
            });
        });

        services.AddSingleton<IPublisher, MassTransitPublisher>();
    }
}