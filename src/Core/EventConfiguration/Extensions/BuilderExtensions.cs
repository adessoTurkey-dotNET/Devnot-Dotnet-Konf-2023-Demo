using System.Reflection;
using EventConfiguration.Base;
using EventConfiguration.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventConfiguration.Extensions;

public static class BuilderExtensions
{
    /// <summary>
    /// Add event configurations. 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection AddEventConfiguration(this IServiceCollection services,
        IConfiguration configuration, Action<EventConfigurationSettings> setupAction = null)
    {
        var bctSpOptions = new EventConfigurationSettings();
        setupAction?.Invoke(bctSpOptions);
        
        var messageBrokerQueueSettings =
            configuration.GetSection("MessageBroker:QueueSettings").Get<MessageBrokerQueueSettings>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(messageBrokerQueueSettings.HostName, messageBrokerQueueSettings.VirtualHost, h =>
                {
                    h.Username(messageBrokerQueueSettings.UserName);
                    h.Password(messageBrokerQueueSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });

            var consumers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => x is { IsClass: true, IsAbstract: false } && typeof(IBaseConsumer).IsAssignableFrom(x));
            
            foreach (var consumer in consumers)
            {
                x.AddConsumer(consumer);
                x.AddRequestClient(consumer);
            }
        });

        return services;
    }
}