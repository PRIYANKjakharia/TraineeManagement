using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.Models;

public class SubmissionFile
{
    [Key]
    public int Id { get; set; }

    public string? OrigionalFileName { get; set; }

    public string? StoredFileName { get; set; }

    public string? ContentType { get; set; }
    public string? Checksum { get; set; }
    public string? UploadedBy { get; set; }

    public long? FileSize { get; set; }
    
    public int? SubmissionId { get; set; }
    public DateTime UploadedDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public Submission? Submission { get; set; }
}
