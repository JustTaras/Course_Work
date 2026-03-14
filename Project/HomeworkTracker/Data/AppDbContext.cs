using Microsoft.EntityFrameworkCore;
using HomeworkTracker.Models;

namespace HomeworkTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Таблиці за твоєю UML-діаграмою
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Submission> Submissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Налаштування спадкування (Table-per-Hierarchy)
        // Всі юзери будуть в одній таблиці, EF сам додасть колонку Discriminator
        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Student>("Student")
            .HasValue<Teacher>("Teacher");

        // 2. Зв'язок Group -> Students (1 до багатьох)
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.SetNull);

        // 3. Зв'язок Teacher -> Assignments (1 до багатьох)
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Teacher)
            .WithMany(t => t.Assignments)
            .HasForeignKey(a => a.TeacherId);

        // 4. Зв'язок Group -> Assignments (1 до багатьох)
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Group)
            .WithMany(g => g.Assignments)
            .HasForeignKey(a => a.GroupId);

        // 5. Зв'язок Assignment -> Submissions (1 до багатьох)
        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId);

        // 6. Зв'язок Student -> Submissions (1 до багатьох)
        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Student)
            .WithMany(st => st.Submissions)
            .HasForeignKey(s => s.StudentId);
    }
}