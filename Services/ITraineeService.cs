using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public interface ITraineeService
{
    Task<List<TraineeResponse>> GetAll();
    Task<TraineeResponse?> GetById(int id);
    Task<TraineeResponse> Create(CreateTraineeRequest request);
    Task<bool> Update(int id , UpdateTraineeRequest request);
    Task<bool> Delete(int id);
    Task<List<TraineeResponse>> Search(String s);
}