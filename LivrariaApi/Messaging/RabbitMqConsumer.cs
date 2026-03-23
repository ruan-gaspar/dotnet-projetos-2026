using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace LivrariaApi.Messaging;

public class RabbitMqConsumer : BackgroundService
{
    private const string QueueName = "livro-criado";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[Consumer] Mensagem recebida: {message}");

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(
            queue: QueueName,
            autoAck: false,
            consumer: consumer
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
