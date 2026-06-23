using TraineeManagement.API.Messages;
 
namespace TraineeManagement.API.Interfaces
{
    public interface IRabbitMqPublisher
    {
        void Publish(SubmissionMessage message);
    }
}