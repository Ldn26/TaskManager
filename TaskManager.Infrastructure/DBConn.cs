using Microsoft.EntityFrameworkCore;

namespace TaskManager.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {} 





  protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configure ProjectMember as composite key
    modelBuilder.Entity<ProjectMember>()
        .HasKey(pm => new { pm.ProjectId, pm.UserId });

    // Project -> ProjectMember
    modelBuilder.Entity<ProjectMember>()
        .HasOne(pm => pm.Project)
        .WithMany(p => p.Members)
        .HasForeignKey(pm => pm.ProjectId);

    // User -> ProjectMember
    modelBuilder.Entity<ProjectMember>()
        .HasOne(pm => pm.User)
        .WithMany(u => u.Projects)
        .HasForeignKey(pm => pm.UserId);

    // TaskItem -> Project
    modelBuilder.Entity<TaskItem>()
        .HasOne(t => t.Project)
        .WithMany(p => p.Tasks)
        .HasForeignKey(t => t.ProjectId);

    // TaskItem -> User (optional)
    modelBuilder.Entity<TaskItem>()
        .HasOne(t => t.AssignedUser)
        .WithMany(u => u.Tasks)
        .HasForeignKey(t => t.AssignedUserId)
        .OnDelete(DeleteBehavior.SetNull);
}



    }
}
