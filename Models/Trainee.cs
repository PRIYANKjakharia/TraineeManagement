using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeManagement.API.Models;

public class Trainee
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // [Required]
    // [MaxLength(50)]
    public string FirstName { get; set; }

    // [Required]
    // [MaxLength(50)]
    public string LastName { get; set; }

    // [Required]
    // [EmailAddress]
    public string Email { get; set; }

    // [Required]
    public string TechStack { get; set; }
    
    // [Required]
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
