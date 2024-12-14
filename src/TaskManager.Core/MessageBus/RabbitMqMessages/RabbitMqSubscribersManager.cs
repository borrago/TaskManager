using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Core.MessageBus.RabbitMqMessages;

public class RabbitMqSubscribersManager : BackgroundService
{
    private readonly IRabbitMqMessages _rabbitMqMessages;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMqSubscribersManager(IRabbitMqMessages rabbitMqMessages, IServiceProvider serviceProvider)
    {
        _rabbitMqMessages = rabbitMqMessages ?? throw new ArgumentNullException(nameof(rabbitMqMessages));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        _rabbitMqMessages.CreateChannelsSubscriber();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            foreach (var channel in _rabbitMqMessages.Channels)
            {
                var consumer = new EventingBasicConsumer(channel.Value);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    await ProcessMessageAsync(message, ea.RoutingKey, stoppingToken);

                    channel.Value.BasicAck(ea.DeliveryTag, multiple: false);
                };

                channel.Value.BasicConsume(queue: channel.Key, autoAck: false, consumer: consumer);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no RabbitMqSubscribersManager: {ex.Message}");
        }
    }

    private async Task ProcessMessageAsync(string message, string queueName, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var subscriber = scope.ServiceProvider.GetServices<IRabbitMqSubrscriber>()
              .FirstOrDefault(f => _rabbitMqMessages.GetQueueName(f.ToString() ?? "") == queueName)
                ?? throw new DomainException("Subscriber não encontrado.");

            await subscriber.Handle(message, stoppingToken);

            Console.WriteLine($"Mensagem processada com sucesso: {queueName} => {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar mensagem: {queueName} => {ex.Message}");
        }
    }
}
