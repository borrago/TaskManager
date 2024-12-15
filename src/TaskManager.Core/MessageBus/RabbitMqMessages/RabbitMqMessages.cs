using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TaskManager.Core.MessageBus.RabbitMqMessages;

public class RabbitMqMessages(IServiceProvider serviceProvider) : IRabbitMqMessages
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public IDictionary<string, IModel> Channels { get; } = new Dictionary<string, IModel>();

    public void CreateChannelsSubscriber()
    {
        using var scope = _serviceProvider.CreateScope();
        var subscriberServices = scope.ServiceProvider.GetServices<IRabbitMqSubrscriber>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        foreach (var subscriber in subscriberServices)
        {
            var channel = CreateModel();
            var queueName = DeclareQueue(channel, subscriber.ToString() ?? "");

            Channels.Add(queueName, channel);
        }
    }

    public void Publish<T>(T message) where T : IMessageBusEventInput
    {
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        var channel = CreateModel();
        var queueName = DeclareQueue(channel, message.ToString() ?? "");

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

        Console.WriteLine($"Mensagem enviada! queueName: {queueName} mensagem: {json}");
    }

    public void Dispose()
    {
        foreach (var channel in Channels)
            GC.SuppressFinalize(channel);
    }

    public string GetQueueName(string input)
    {
        var segments = input.Split('.');
        return segments[3].Replace("Subscriber", "").Replace("Event", "");
    }

    #region Private Metods

    private string DeclareQueue(IModel channel, string queueName)
    {
        var name = GetQueueName(queueName);
        channel.QueueDeclare(queue: name,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        channel.ConfirmSelect();
        channel.BasicAcks += Evento_Confirmacao!;
        channel.BasicNacks += Evento_NaoConfirmacao!;

        return name;
    }

    private IModel CreateModel()
    {
        using var scope = _serviceProvider.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var factory = new ConnectionFactory()
        {
            HostName = configuration["Rabbitmq:HostName"],
            Port = int.Parse(configuration["Rabbitmq:Port"] ?? "0"),
            UserName = configuration["Rabbitmq:UserName"],
            Password = configuration["Rabbitmq:Password"]
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        return channel;
    }

    private void Evento_NaoConfirmacao(object sender, BasicNackEventArgs e)
    {
        Console.WriteLine("Nack");
    }

    private void Evento_Confirmacao(object sender, BasicAckEventArgs e)
    {
        Console.WriteLine("Ack");
    }

    #endregion
}