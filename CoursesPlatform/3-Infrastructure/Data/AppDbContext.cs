using _2_Domain.Entities;
using _3_Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace _3_Infrastructure.Data;

public class AppDbContext :  DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<User> Users => Set<User>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>()
            .HasQueryFilter(c => !c.IsDeleted);

        modelBuilder.Entity<Lesson>()
            .HasQueryFilter(l => !l.IsDeleted);

        modelBuilder.Entity<Course>()
            .HasMany(c => c.Lessons)
            .WithOne(l => l.Course!)
            .HasForeignKey(l => l.CourseId);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
        });
        
        UserSeed.Seed(modelBuilder);
    }
}