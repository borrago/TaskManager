using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskManager.Core.MessageBus.RabbitMqMessages;

namespace TaskManager.Core.MessageBus;

public class MessageBus : IMessageBus
{
    private readonly ILogger<IMessageBus> _logger;
    private readonly IServiceProvider _services;
    private readonly IRabbitMqMessages _rabbitMqMessages;

    public MessageBus(IServiceProvider serviceProvider, ILogger<IMessageBus> logger, IRabbitMqMessages rabbitMqMessages)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _rabbitMqMessages = rabbitMqMessages ?? throw new ArgumentNullException(nameof(rabbitMqMessages));

        _logger.LogInformation($"[{nameof(MessageBus)}]: metodo Broker Construtor");
    }

    public async Task<IMessageBusEventResult> PublishAsync(IMessageBusEventInput @event, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"\"{nameof(PublishAsync)}\" tentando publicar a mensagem: {@event.ToString()} => {JsonConvert.SerializeObject(@event)}");

            _rabbitMqMessages.Publish(@event);

            await Task.Delay(1, cancellationToken);

            return new MessageBusEventResult(0, "Mensagem Enviada com sucesso.");
        }
        catch (Exception e)
        {
            var msg = $"[{nameof(PublishAsync)}]: não foi possivel publicar a mensagem: {@event.ToString()} => {JsonConvert.SerializeObject(@event)} : {JsonConvert.SerializeObject(e)}";

            _logger.LogError(msg);

            return new MessageBusEventResult(1, msg);
        }
    }
}