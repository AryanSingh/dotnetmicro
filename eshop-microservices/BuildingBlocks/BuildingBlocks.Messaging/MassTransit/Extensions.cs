using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    /// <summary>
    /// Adds and configures MassTransit with RabbitMQ as the message broker.
    /// Optionally registers consumers from the provided assembly.
    /// RabbitMQ connection settings are read from configuration section 'MessageBroker'.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="assembly">Optional assembly to scan for consumers.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config => 
        {
            config.SetKebabCaseEndpointNameFormatter();
            if (assembly != null)
            {
                config.AddConsumers(assembly);
            }
            config.UsingRabbitMq((context, cfg) =>
            {
                // Read RabbitMQ settings from configuration
                var brokerSection = configuration.GetSection("MessageBroker");
                var host = brokerSection["Host"] ?? "amqp://localhost:5672";
                var username = brokerSection["Username"] ?? "guest";
                var password = brokerSection["Password"] ?? "guest";
                cfg.Host(new Uri(host), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                // Optionally configure more MassTransit features here (e.g., retry, outbox, etc.)
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}