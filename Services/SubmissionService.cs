
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace TraineeManagement.API.Services;

public class SubmissionService : ISubmissionService
{
    private readonly AppDbContext _context;
    private readonly ILogger<SubmissionService> _logger;
    public SubmissionService(AppDbContext context , ILogger<SubmissionService> logger)
    {
        _context = context;
        _logger = logger;
    }


    // create
    public async Task<SubmissionResponse> CreateAsync(CreateSubmissionRequest request)
    {   
        var AssignmentExists = await _context.TaskAssignments.FirstOrDefaultAsync(x => x.Id == request.TaskAssignmentId);
        
        if(AssignmentExists == null) return null;

        var submission = new Submission
        {
            TaskAssignmentId = request.TaskAssignmentId,
            SubmissionUrl = request.SubmissionUrl,
            Notes = request.Notes,
            Status = request.Status,
            SubmissionDate = request.SubmissionDate,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow

        };
        // trainees.Add(learningTask);
        await _context.Submissions.AddAsync(submission);
        await _context.SaveChangesAsync();
        _logger.LogInformation("task Assigned done with assignmentId "+ submission.TaskAssignmentId );
        return new SubmissionResponse
        {
            Id= submission.Id,
            TaskAssignmentId = submission.TaskAssignmentId,
            SubmissionUrl = submission.SubmissionUrl,
            Notes = submission.Notes,
            Status = submission.Status,
            SubmissionDate = submission.SubmissionDate,
        };
    }


// get
    public async Task<List<SubmissionResponse>> GetAllAsync()
    {
        _logger.LogInformation("Info Displayed");

        

        // .Include(e=>e.Trainee)
        return await _context.Submissions.Include(x => x.TaskAssignment).Select(t => new SubmissionResponse
        {
            Id= t.Id,
            TaskAssignmentId = t.TaskAssignmentId,
            SubmissionUrl = t.SubmissionUrl,
            Notes = t.Notes,
            Status = t.Status,
            SubmissionDate = t.SubmissionDate,
            // Trainee = t.Trainee,
        }).ToListAsync();
    }

    public async Task<SubmissionResponse?> GetByIdAsync(int id)
    {
        var t = await _context.Submissions.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return null;
        }
        _logger.LogInformation("Info Displayed");
        return new SubmissionResponse
        {
            Id= t.Id,
            TaskAssignmentId = t.TaskAssignmentId,
            SubmissionUrl = t.SubmissionUrl,
            Notes = t.Notes,
            Status = t.Status,
            SubmissionDate = t.SubmissionDate,
        };
    }

}
