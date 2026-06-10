
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagement.API.Services;

public class TraineeService : ITraineeService
{
    private readonly AppDbContext _context;
    public TraineeService(AppDbContext context)
    {
        _context = context;
    }


    // create
    public async Task<TraineeResponse> Create(CreateTraineeRequest request)
    {
        var trainee = new Trainee
        {
            // Id = nextId++,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            TechStack = request.TechStack,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow

        };
        // trainees.Add(trainee);
        await _context.Trainees.AddAsync(trainee);
        await _context.SaveChangesAsync();
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






    // delete
    public async Task<bool> Delete(int id)
    {
        var t = await _context.Trainees.FindAsync(id);
        if(t == null)return false;
        _context.Trainees.Remove(t);
        await _context.SaveChangesAsync();
        return true;
    }





// get
    public async Task<List<TraineeResponse>> GetAll()
    {
        var query = _context.Trainees.AsQueryable();
        return await _context.Trainees.Select(t => new TraineeResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            TechStack = t.TechStack,
            Status = t.Status
        }).ToListAsync();
    }

    public async Task<TraineeResponse?> GetById(int id)
    {
        var t = await _context.Trainees.FindAsync(id);
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




// update
    public async Task<bool> Update (int id, UpdateTraineeRequest request)
    {
        var t = await _context.Trainees.FindAsync(id);
        if(t == null)return false;

        t.FirstName = request.FirstName;
        t.LastName = request.LastName;
        t.Email = request.Email;
        t.TechStack = request.TechStack;
        t.Status = request.Status;
        t.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }





// search
    public async Task<List<TraineeResponse>> Search(String search)
    {
        var t = await _context.Trainees.Where(e=>e.FirstName==search || e.LastName==search || e.Email==search || e.TechStack==search).Select( e => new TraineeResponse
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            TechStack = e.TechStack,
            Status = e.Status
        }).ToListAsync();

        if(t.Count == 0)
        {
            return null;
        }
        return t;
    }
}
