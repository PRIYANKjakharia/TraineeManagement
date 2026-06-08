using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Models;
namespace TraineeManagement.API.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Trainee> Trainees {get ; set;}
}