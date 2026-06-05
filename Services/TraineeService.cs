
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public class TraineeService : ITraineeService
{
    private static List<Trainee> trainees = new();
    private static int nextId = 1;
    public TraineeResponse Create(CreateTraineeRequest request)
    {
        var trainee = new Trainee
        {
            Id = nextId++,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            TechStack = request.TechStack,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow

        };
        trainees.Add(trainee);
        return new TraineeResponse
        {
            Id = trainee.Id,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            TechStack = trainee.TechStack,
            Status = trainee.Status
        };
    }

    public bool Delete(int id)
    {
        var t = trainees.FirstOrDefault(trainee => trainee.Id == id);
        if(t == null)return false;
        trainees.Remove(t);
        return true;
    }

    public List<TraineeResponse> GetAll()
    {
        return trainees.Select(t => new TraineeResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            TechStack = t.TechStack,
            Status = t.Status
        }).ToList();
    }

    public TraineeResponse? GetById(int id)
    {
        var t = trainees.FirstOrDefault(trainee => trainee.Id == id);
        if(t == null)return null;
        return new TraineeResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            TechStack = t.TechStack,
            Status = t.Status
        };
    }

    public bool Update(int id, UpdateTraineeRequest request)
    {
        var t = trainees.FirstOrDefault(trainee => trainee.Id == id);
        if(t == null)return false;
            t.FirstName = request.FirstName;
            t.LastName = request.LastName;
            t.Email = request.Email;
            t.TechStack = request.TechStack;
            t.Status = request.Status;
            t.UpdatedDate = DateTime.UtcNow;
        return true;
    }
}
