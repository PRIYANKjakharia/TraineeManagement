
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;

namespace TraineeManagement.API.Services;

public class TraineeService : ITraineeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TraineeService> _logger;
    public TraineeService(AppDbContext context , ILogger<TraineeService> logger)
    {
        _context = context;
        _logger = logger;
    }


    // create
    public async Task<TraineeResponse> Create(CreateTraineeRequest request)
    {   
        var EmailExists = await _context.Trainees.FirstOrDefaultAsync(k => k.Email.ToLower() == request.Email.ToLower());
        if(EmailExists != null){
            _logger.LogCritical("Email already exists");
            return null;
        }
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
        _logger.LogInformation("User Created with email "+request.Email);
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
        if(t == null){
            _logger.LogCritical("Id not found");
            return false;
        }
        _context.Trainees.Remove(t);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User with id "+id+" deleted");
        return true;
    }





// get
    // public async Task<List<TraineeResponse>> GetAll()
    // {
    //     var query = _context.Trainees.AsQueryable();
    //     _logger.LogInformation("Info Displayed");
    //     return await _context.Trainees.Select(t => new TraineeResponse
    //     {
    //         Id = t.Id,
    //         FirstName = t.FirstName,
    //         LastName = t.LastName,
    //         Email = t.Email,
    //         TechStack = t.TechStack,
    //         Status = t.Status
    //     }).ToListAsync();
    // }

    public async Task<TraineeResponse?> GetById(int id)
    {
        var t = await _context.Trainees.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return null;
        }
        _logger.LogInformation("Info Displayed");
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
    public async Task<string> Update (int id, UpdateTraineeRequest request)
    {
        var t = await _context.Trainees.FindAsync(id);
        var EmailExists = await _context.Trainees.FirstOrDefaultAsync(k => k.Id != id && k.Email.ToLower() == request.Email.ToLower());
        if(EmailExists != null){
            _logger.LogCritical("Email already exists");
            return "Email already exists";
        }
        if(t == null){
            _logger.LogCritical("Id not found");
            return "Id Not Found";
        }

        t.FirstName = request.FirstName;
        t.LastName = request.LastName;
        t.Email = request.Email;
        t.TechStack = request.TechStack;
        t.Status = request.Status;
        t.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("User Updated with Id "+id);
        return "Updated SucessFully";
    }





// search
    // public async Task<List<TraineeResponse>> Search(String search)
    // {
    //     var t = await _context.Trainees.Where(e=>e.FirstName==search || e.LastName==search || e.Email==search || e.TechStack==search).Select( e => new TraineeResponse
    //     {
    //         Id = e.Id,
    //         FirstName = e.FirstName,
    //         LastName = e.LastName,
    //         Email = e.Email,
    //         TechStack = e.TechStack,
    //         Status = e.Status
    //     }).ToListAsync();

    //     if(t.Count == 0)
    //     {
    //         _logger.LogCritical("Id not found");
    //         return null;
    //     }
    //     _logger.LogInformation("Search Displayed");
    //     return t;
    // }

    public async Task<PagedResponse<TraineeResponse>> GetAllAsync(TraineeQueryParameters query)
    {
        var trainees = _context.Trainees.AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            trainees = trainees.Where(t => t.FirstName.Contains(query.Search) || t.LastName.Contains(query.Search));
        }
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            trainees = trainees.Where(t => t.Status == query.Status);
        }
        int page = query.PageSize * (query.PageNumber-1);
        List<TraineeResponse> ret =  await trainees.Select( t => new TraineeResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            TechStack = t.TechStack,
            Status = t.Status
        }).Skip(page).Take(query.PageSize).ToListAsync();

        _logger.LogInformation("request for page no. "+query.PageNumber+" with page size "+query.PageSize+" fetched and Displayed");
        return new PagedResponse<TraineeResponse>
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalRecords = ret.Count,
            Data = ret
        };
    }

}
