using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace LivrariaApi.Messaging;

public class RabbitMqUpdateConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var queueName = "livro-atualizado";

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("=================================");
                Console.WriteLine("[CONSUMER] LIVRO ATUALIZADO");
                Console.WriteLine("[EMAIL SIMULADO]");
                Console.WriteLine($"Mensagem recebida: {message}");
                Console.WriteLine("Notificação de atualização enviada!");
                Console.WriteLine("=================================");

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[RABBITMQ CONSUMER ERROR - livro-atualizado]");
            Console.WriteLine(ex.Message);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
