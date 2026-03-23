using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace LivrariaApi.Messaging;

public static class RabbitMqProducer
{
    private const string QueueName = "livro-criado";

    public static void Publish<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"

        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: "",
            routingKey: QueueName,
            basicProperties: null,
            body: body
        );
    }
}
