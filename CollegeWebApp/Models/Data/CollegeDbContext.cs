using CollegeWebApp.Models;
using Microsoft.EntityFrameworkCore;

public class CollegeDbContext : DbContext
{
    public CollegeDbContext(DbContextOptions<CollegeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Module> Modules { get; set; }
}
