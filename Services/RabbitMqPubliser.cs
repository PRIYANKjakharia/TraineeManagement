using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TraineeManagement.API.Interfaces;
using TraineeManagement.API.Messages;
 
namespace TraineeManagement.API.Services
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        public void Publish(SubmissionMessage message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
 
            using var connection = factory.CreateConnection();
 
            using var channel = connection.CreateModel();
 
            channel.QueueDeclare(
                queue: "submission_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
 
            var json = JsonSerializer.Serialize(message);
 
            var body = Encoding.UTF8.GetBytes(json);
 
            channel.BasicPublish(
                exchange: "",
                routingKey: "submission_queue",
                basicProperties: null,
                body: body);
        }
    }
}
 