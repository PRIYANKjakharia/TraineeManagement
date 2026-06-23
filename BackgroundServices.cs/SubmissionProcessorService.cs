using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TraineeManagement.API.Messages;
 
namespace TraineeManagement.API.BackgroundServices
{
    public class SubmissionProcessorService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
 
            var connection = factory.CreateConnection();
 
            var channel = connection.CreateModel();
 
            channel.QueueDeclare(
                queue: "submission_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
 
            var consumer = new EventingBasicConsumer(channel);
 
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
 
                var json = Encoding.UTF8.GetString(body);
 
                var message = JsonSerializer.Deserialize<SubmissionMessage>(json);
 
                Console.WriteLine($"Processing submission {message!.SubmissionId}");
                Console.WriteLine(" changing status to Processing");
                Thread.Sleep(5000);
                Console.WriteLine("changing status to completed");
                Console.WriteLine("ddone");
 
                channel.BasicAck(ea.DeliveryTag, false);
            };
 
            channel.BasicConsume(
                queue: "submission_queue",
                autoAck: false,
                consumer: consumer);
 
            return Task.CompletedTask;
        }
    }
}
 