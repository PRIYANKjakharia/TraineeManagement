using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public interface ITraineeService
{
    List<TraineeResponse> GetAll();
    TraineeResponse? GetById(int id);
    TraineeResponse Create(CreateTraineeRequest request);
    bool Update(int id , UpdateTraineeRequest request);
    bool Delete(int id);
}