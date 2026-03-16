using HomeworkTracker.Data;
using HomeworkTracker.Models;
using HomeworkTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeworkTracker.Tests;

public class RepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // --- USER REPOSITORY (5 tests) ---
    [Fact] public async Task AddUserAsync_SavesUser() {
        var ctx = GetDbContext(); var repo = new UserRepository(ctx);
        await repo.AddUserAsync(new Student { Name = "St", Email = "e" });
        await ctx.SaveChangesAsync();
        Assert.Equal(1, await ctx.Users.CountAsync());
    }

    [Fact] public async Task GetUserByIdAsync_ReturnsCorrectUser() {
        var ctx = GetDbContext(); var repo = new UserRepository(ctx);
        var u = new Teacher { Name = "T" };
        ctx.Users.Add(u); await ctx.SaveChangesAsync();
        var res = await repo.GetUserByIdAsync(u.Id);
        Assert.NotNull(res); Assert.Equal("T", res.Name);
    }

    [Fact] public async Task GetUserByIdAsync_ReturnsNull_ForInvalidId() {
        var ctx = GetDbContext(); var repo = new UserRepository(ctx);
        var res = await repo.GetUserByIdAsync(999);
        Assert.Null(res);
    }

    [Fact] public async Task GetAllUsersAsync_ReturnsAll() {
        var ctx = GetDbContext(); var repo = new UserRepository(ctx);
        ctx.Users.AddRange(new Student(), new Teacher()); await ctx.SaveChangesAsync();
        var res = await repo.GetAllUsersAsync();
        Assert.Equal(2, res.Count);
    }

    [Fact] public async Task GetAllUsersAsync_ReturnsEmpty_WhenNoUsers() {
        var ctx = GetDbContext(); var repo = new UserRepository(ctx);
        var res = await repo.GetAllUsersAsync();
        Assert.Empty(res);
    }

    // --- ASSIGNMENT REPOSITORY (5 tests) ---
    [Fact] public async Task AddAssignmentAsync_SavesToDb() {
        var ctx = GetDbContext(); var repo = new AssignmentRepository(ctx);
        await repo.AddAssignmentAsync(new Assignment { Title = "A" }); await ctx.SaveChangesAsync();
        Assert.Equal(1, await ctx.Assignments.CountAsync());
    }

    [Fact] public async Task GetAssignmentByIdAsync_ReturnsAssignment() {
        var ctx = GetDbContext(); var repo = new AssignmentRepository(ctx);
        var a = new Assignment { Title = "A" }; ctx.Assignments.Add(a); await ctx.SaveChangesAsync();
        var res = await repo.GetByIdAsync(a.Id);
        Assert.Equal("A", res?.Title);
    }

    [Fact] public async Task GetAllAssignmentsAsync_IncludesGroupData() {
        var ctx = GetDbContext(); var repo = new AssignmentRepository(ctx);
        var g = new Group { Name = "G1" };
        ctx.Assignments.Add(new Assignment { Title = "A", Group = g }); await ctx.SaveChangesAsync();
        var res = await repo.GetAllAsync();
        Assert.NotNull(res.First().Group);
        Assert.Equal("G1", res.First().Group?.Name);
    }

    [Fact] public async Task GetAssignmentByIdAsync_ReturnsNullIfMissing() {
        var ctx = GetDbContext(); var repo = new AssignmentRepository(ctx);
        Assert.Null(await repo.GetByIdAsync(1));
    }

    [Fact] public async Task GetAllAssignmentsAsync_ReturnsEmptyIfNone() {
        var ctx = GetDbContext(); var repo = new AssignmentRepository(ctx);
        Assert.Empty(await repo.GetAllAsync());
    }

    // --- SUBMISSION REPOSITORY (5 tests) ---
    [Fact] public async Task AddSubmissionAsync_Saves() {
        var ctx = GetDbContext(); var repo = new SubmissionRepository(ctx);
        await repo.AddSubmissionAsync(new Submission { Content = "C" }); await ctx.SaveChangesAsync();
        Assert.Equal(1, await ctx.Submissions.CountAsync());
    }

    [Fact] public async Task GetSubmissionById_ReturnsEntity() {
        var ctx = GetDbContext(); var repo = new SubmissionRepository(ctx);
        var s = new Submission { Content = "C" }; ctx.Submissions.Add(s); await ctx.SaveChangesAsync();
        var res = await repo.GetByIdAsync(s.Id);
        Assert.Equal("C", res?.Content);
    }

    [Fact] 
public async Task GetByAssignmentId_ReturnsCorrectList() 
{
    var ctx = GetDbContext(); 
    var repo = new SubmissionRepository(ctx);

    // 1. Спочатку створюємо реального студента для бази
    var testStudent = new Student { Name = "Тест", Email = "test@test.com" };
    ctx.Users.Add(testStudent); 
    // Зберігаємо, щоб студент отримав свій Id
    await ctx.SaveChangesAsync(); 

    // 2. Тепер створюємо роботи, прив'язані саме до цього студента
    var sub1 = new Submission { AssignmentId = 1, StudentId = testStudent.Id, Content = "Текст 1" };
    var sub2 = new Submission { AssignmentId = 2, StudentId = testStudent.Id, Content = "Текст 2" };
    
    ctx.Submissions.AddRange(sub1, sub2);
    await ctx.SaveChangesAsync();
    
    // 3. Перевіряємо
    var res = await repo.GetByAssignmentIdAsync(1);
    Assert.Single(res); // Тепер список точно не буде порожнім!
}

    [Fact] public async Task GetByAssignmentId_IncludesStudentData() {
        var ctx = GetDbContext(); var repo = new SubmissionRepository(ctx);
        var st = new Student { Name = "Bob" };
        ctx.Submissions.Add(new Submission { AssignmentId = 1, Student = st }); await ctx.SaveChangesAsync();
        var res = await repo.GetByAssignmentIdAsync(1);
        Assert.Equal("Bob", res.First().Student?.Name);
    }

    [Fact] public async Task GetByAssignmentId_ReturnsEmptyIfNoneMatch() {
        var ctx = GetDbContext(); var repo = new SubmissionRepository(ctx);
        var res = await repo.GetByAssignmentIdAsync(99);
        Assert.Empty(res);
    }

    // --- GROUP REPOSITORY (5 tests) ---
    [Fact] public async Task AddGroupAsync_Saves() {
        var ctx = GetDbContext(); var repo = new GroupRepository(ctx);
        await repo.AddGroupAsync(new Group { Name = "G" }); await ctx.SaveChangesAsync();
        Assert.Equal(1, await ctx.Groups.CountAsync());
    }

    [Fact] public async Task GetGroupById_IncludesStudents() {
        var ctx = GetDbContext(); var repo = new GroupRepository(ctx);
        var g = new Group { Name = "G", Students = new List<Student> { new Student() } };
        ctx.Groups.Add(g); await ctx.SaveChangesAsync();
        var res = await repo.GetByIdAsync(g.Id);
        Assert.Single(res?.Students!);
    }

    [Fact] public async Task GetGroupById_ReturnsNullIfNotFound() {
        var ctx = GetDbContext(); var repo = new GroupRepository(ctx);
        Assert.Null(await repo.GetByIdAsync(1));
    }

    [Fact] public async Task GetAllGroups_ReturnsAll() {
        var ctx = GetDbContext(); var repo = new GroupRepository(ctx);
        ctx.Groups.AddRange(new Group(), new Group()); await ctx.SaveChangesAsync();
        Assert.Equal(2, (await repo.GetAllAsync()).Count);
    }

    [Fact] public async Task UnitOfWork_CommitAsync_SavesChanges() {
        var ctx = GetDbContext(); var uow = new UnitOfWork(ctx);
        await uow.Groups.AddGroupAsync(new Group { Name = "G" });
        await uow.CommitAsync();
        Assert.Equal(1, await ctx.Groups.CountAsync());
    }
}