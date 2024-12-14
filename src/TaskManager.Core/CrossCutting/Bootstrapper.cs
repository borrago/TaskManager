using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManager.Core.MessageBus;
using TaskManager.Core.MessageBus.RabbitMqMessages;

namespace TaskManager.Core.CrossCutting;

public static class Bootstrapper
{
    private const string NAME_CORS_POLICY = "AllowAll";

    public static IServiceCollection Register(this IServiceCollection services)
    {
        RegisterEndpoints(services);

        RegisterCORS(services);

        RegisterSubscribers(services);

        return services;
    }

    public static IApplicationBuilder Use(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseCors(NAME_CORS_POLICY);

        app.UseAuthorization();

        return app;
    }

    private static IServiceCollection RegisterEndpoints(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API direcionada ao cadastro de tarefas", Version = "v1" });
        });

        return services;
    }

    private static IServiceCollection RegisterCORS(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(NAME_CORS_POLICY, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        return services;
    }

    private static IServiceCollection RegisterSubscribers(IServiceCollection services)
    {
        services.AddSingleton<IMessageBus, MessageBus.MessageBus>();
        services.AddSingleton<IRabbitMqMessages, RabbitMqMessages>();

        var assembly = AppDomain.CurrentDomain.Load("TaskManager.Application");

        var concreteBrokerTopicConsumers = assembly.ExportedTypes
            .Select(t => t.GetTypeInfo())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(IRabbitMqSubrscriber)));

        foreach (var concreteBrokerTopicConsumer in concreteBrokerTopicConsumers)
            services.AddTransient(typeof(IRabbitMqSubrscriber), concreteBrokerTopicConsumer.AsType());

        services.AddHostedService<RabbitMqSubscribersManager>();

        return services;
    }
}
