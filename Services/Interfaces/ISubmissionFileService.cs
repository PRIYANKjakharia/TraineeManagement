using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;

namespace TraineeManagement.API.Services;

public interface ISubmissionFileService
{
    Task<UploadFileResponse?> UploadAsync(int submissionId , IFormFile file);
}







