
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;

namespace TraineeManagement.API.Services;

public class MentorService : IMentorService
{
    private readonly AppDbContext _context;
    private readonly ILogger<MentorService> _logger;
    public MentorService(AppDbContext context , ILogger<MentorService> logger)
    {
        _context = context;
        _logger = logger;
    }


    // create
    public async Task<MentorResponse> CreateAsync(CreateMentorRequest request)
    {   
        var emailExists = await _context.Mentors.FirstOrDefaultAsync(k => k.Email.ToLower() == request.Email.ToLower());
        if(emailExists != null){
            _logger.LogCritical("Email already exists");
            return null;
        }
        var mentor = new Mentor
        {
            // Id = nextId++,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Expertise = request.Expertise,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow

        };
        // trainees.Add(mentor);
        await _context.Mentors.AddAsync(mentor);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Mentor Created with email "+request.Email);
        return new MentorResponse
        {
            Id = mentor.Id,
            FirstName = mentor.FirstName,
            LastName = mentor.LastName,
            Email = mentor.Email,
            Expertise = mentor.Expertise,
            Status = mentor.Status
        };
    }






    // delete
    public async Task<bool> DeleteAsync(int id)
    {
        var t = await _context.Mentors.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return false;
        }
        _context.Mentors.Remove(t);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Mentor with id "+id+" deleted");
        return true;
    }





// get
    public async Task<List<MentorResponse>> GetAllAsync()
    {
        _logger.LogInformation("Info Displayed");
        return await _context.Mentors.Select(t => new MentorResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            Expertise = t.Expertise,
            Status = t.Status
        }).ToListAsync();
    }

    public async Task<MentorResponse?> GetByIdAsync(int id)
    {
        var t = await _context.Mentors.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return null;
        }
        _logger.LogInformation("Info Displayed");
        return new MentorResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            Expertise = t.Expertise,
            Status = t.Status
        };
    }




// update
    public async Task<string> UpdateAsync (int id, UpdateMentorRequest request)
    {
        var t = await _context.Mentors.FindAsync(id);
        var emailExists = _context.Mentors.FirstOrDefault(k => k.Email.ToLower() == request.Email.ToLower());
        if(emailExists != null){
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
        t.Expertise = request.Expertise;
        t.Status = request.Status;
        t.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Mentor Updated with Id "+id);
        return "Updated SucessFully";
    }


    // search
    // public async Task<List<MentorResponse>> Search(String search)
    // {
    //     var t = await _context.Mentors.Where(e=>e.FirstName==search || e.LastName==search || e.Email==search || e.Expertise==search).Select( e => new MentorResponse
    //     {
    //         Id = e.Id,
    //         FirstName = e.FirstName,
    //         LastName = e.LastName,
    //         Email = e.Email,
    //         Expertise = e.Expertise,
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

    // public async Task<PagedResponse<MentorResponse>> GetAllAsync()
    // {
    //     var mentor = _context.Mentors.AsQueryable();
    // if (!string.IsNullOrWhiteSpace(query.Search))
    // {
    //     trainees = trainees.Where(t => t.FirstName.Contains(query.Search) || t.LastName.Contains(query.Search));
    // }
    // if (!string.IsNullOrWhiteSpace(query.Status))
    // {
    //     trainees = trainees.Where(t => t.Status == query.Status);
    // }
    // int page = query.PageSize * (query.PageNumber-1);
    // List<MentorResponse> ret =  await trainees.Select( t => new MentorResponse
    // {
    //     Id = t.Id,
    //     FirstName = t.FirstName,
    //     LastName = t.LastName,
    //     Email = t.Email,
    //     Expertise = t.Expertise,
    //     Status = t.Status
    // }).Skip(page).Take(query.PageSize).ToListAsync();

    // _logger.LogInformation("request for page no. "+query.PageNumber+" with page size "+query.PageSize+" fetched and Displayed");
    //     return new PagedResponse<MentorResponse>
    //     {
    //         PageNumber = query.PageNumber,
    //         PageSize = query.PageSize,
    //         TotalRecords = ret.Count,
    //         Data = ret
    //     };
    // }
}
