using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManager.Core.Infra;
using TaskManager.Core.MessageBus;
using TaskManager.Core.MessageBus.RabbitMqMessages;

namespace TaskManager.Core.CrossCutting;

public static class Bootstrapper
{
    private const string NAME_CORS_POLICY = "AllowAll";

    public static IServiceCollection Register(this IServiceCollection services)
    {
        RegisterEndpoints(services);
        AddMediatorHandlers(services);
        RegisterInfra(services);

        RegisterCORS(services);

        RegisterSubscribers(services);
        return services;
    }

    private static IServiceCollection RegisterInfra(this IServiceCollection services)
    {
        var assembly = AppDomain.CurrentDomain.Load("TaskManager.Infra");
        var types = assembly.GetTypes();
        var repositoryInterfaces = new[]
        {
            typeof(IGenericRepository<>),
            typeof(IGenericReadRepository<>)
        };

        foreach (var repositoryInterface in repositoryInterfaces)
        {
            var repositories = types
                .Where(type => !type.IsInterface && !type.IsAbstract && type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == repositoryInterface));

            foreach (var repository in repositories)
            {
                var specificInterface = repository.GetInterfaces()
                    .FirstOrDefault(i => i != repositoryInterface && i.GetInterfaces()
                        .Any(ii => ii.IsGenericType && ii.GetGenericTypeDefinition() == repositoryInterface));

                if (specificInterface != null)
                    services.AddScoped(specificInterface, repository);
            }
        }

        return services;
    }

    private static IServiceCollection AddMediatorHandlers(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        var assembly = AppDomain.CurrentDomain.Load("TaskManager.Application");

        var classTypes = assembly.ExportedTypes.Select(t => t.GetTypeInfo()).Where(t => t.IsClass && !t.IsAbstract);

        foreach (var type in classTypes)
        {
            var handlerInterfaceNames = new List<string>
            {
                typeof(IRequestHandler<,>).Name,
                typeof(INotificationHandler<>).Name
            };

            var handlerTypes = type.ImplementedInterfaces
                .Select(i => i.GetTypeInfo())
                .Where(i => handlerInterfaceNames.Contains(i.Name));

            foreach (var handlerType in handlerTypes)
                services.AddTransient(handlerType.AsType(), type.AsType());
        }

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
