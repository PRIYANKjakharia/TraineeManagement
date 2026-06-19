using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Data;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;

namespace TraineeManagement.API.Services;

public class SubmissionFileService : ISubmissionFileService
{
    private readonly AppDbContext _context;
    private readonly IFileStorageService _storage;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public SubmissionFileService(AppDbContext context , IFileStorageService storage , IConfiguration configuration , IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _storage = storage;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UploadFileResponse?> UploadAsync(int submissionId, IFormFile file)
    {
        var submission = await _context.Submissions.FirstOrDefaultAsync(s => s.Id == submissionId);
        if(submission == null)
        {
            return null;
        }
        if(file.Length == 0)
        {
            throw new Exception("File Is Empty");
        }


        string[] allowedExtensions = _configuration.GetSection("FileValidation:AllowedExtensions").Get<string[]>()!;
        string extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
        {
            throw new Exception("Invalid file type");
        }



        long maxSize = _configuration.GetValue<long>("FileValidation:MaxSizeMB") *1024 * 1024;
        if(file.Length > maxSize)
        {
            throw new Exception("File too Large");
        }



        string storedFileName = await _storage.SaveFileAsync(file);



        string checksum = await GenerateChecksum(file);
        // var user = _httpContextAccessor.HttpContext!.User;
        // var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        // var Username = user.FindFirst(ClaimTypes.Name)!.Value;
        var submissionFile = new SubmissionFile
        {
            SubmissionId = submissionId,
        
            OriginalFileName = file.FileName,
        
            StoredFileName = storedFileName,
        
            ContentType = file.ContentType,
        
            FileSize = file.Length,
        
            Checksum = checksum,
        
            // UploadedByUser = Username,

            // UploadedByUserId = userId,
            UploadedByUser = "Admin",
        
            UploadedDate = DateTime.UtcNow,
        
            UpdatedDate = DateTime.UtcNow
        };

        _context.SubmissionFiles.Add(submissionFile);
        await _context.SaveChangesAsync();


        return new UploadFileResponse
        {
            Id = submissionFile.Id,
        
            OriginalFileName = submissionFile.OriginalFileName,
        
            StoredFileName = submissionFile.StoredFileName,
        
            FileSize = submissionFile.FileSize,
        
            ContentType = submissionFile.ContentType,
        
            Checksum = submissionFile.Checksum
        };
    }
    public async Task<(Stream stream, string contentType, string fileName)?> DownloadAsync(int fileId)
    {
        var file = await _context.SubmissionFiles.FirstOrDefaultAsync(x => x.Id == fileId);
    
        if (file == null)
        {
            return null;
        }
    
        bool exists = await _storage.ExistsAsync(file.StoredFileName);
    
        if (!exists)
        {
            return null;
        }
    
        var stream = await _storage.OpenReadAsync(file.StoredFileName);
    
        return ( stream , file.ContentType , file.OriginalFileName );
    }
    public async Task<bool> DeleteAsync(int fileId)
    {
        var file = await _context.SubmissionFiles.FirstOrDefaultAsync(x => x.Id == fileId);
    
        if (file == null)
        {
            return false;
        }
        await _storage.DeleteAsync(file.StoredFileName);
        _context.SubmissionFiles.Remove(file);
        await _context.SaveChangesAsync();
    
        return true;
    }
    private static async Task<string> GenerateChecksum(IFormFile file)
    {
        using var sha256 = SHA256.Create();
    
        using var stream = file.OpenReadStream();
    
        byte[] hash = await sha256.ComputeHashAsync(stream);
    
        return Convert.ToHexString(hash);
    }
}