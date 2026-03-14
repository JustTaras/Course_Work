using HomeworkTracker.Data;
using HomeworkTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeworkTracker.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    
    public IUserRepository Users { get; }
    public IAssignmentRepository Assignments { get; }
    public ISubmissionRepository Submissions { get; }
    public IGroupRepository Groups { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Assignments = new AssignmentRepository(_context);
        Submissions = new SubmissionRepository(_context);
        Groups = new GroupRepository(_context);
    }

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
}

// РЕАЛІЗАЦІЯ РЕПОЗИТОРІЇВ
public class UserRepository(AppDbContext context) : IUserRepository {
    public async Task AddUserAsync(User user) => await context.Users.AddAsync(user);
    public async Task<User?> GetUserByIdAsync(int id) => await context.Users.FindAsync(id);
    public async Task<List<User>> GetAllUsersAsync() => await context.Users.ToListAsync();
}

public class AssignmentRepository(AppDbContext context) : IAssignmentRepository {
    public async Task AddAssignmentAsync(Assignment a) => await context.Assignments.AddAsync(a);
    public async Task<Assignment?> GetByIdAsync(int id) => await context.Assignments.FindAsync(id);
    public async Task<List<Assignment>> GetAllAsync() => await context.Assignments.Include(a => a.Group).ToListAsync();
}

public class SubmissionRepository(AppDbContext context) : ISubmissionRepository {
    public async Task AddSubmissionAsync(Submission s) => await context.Submissions.AddAsync(s);
    public async Task<Submission?> GetByIdAsync(int id) => await context.Submissions.FindAsync(id);
    public async Task<List<Submission>> GetByAssignmentIdAsync(int id) => 
        await context.Submissions.Where(s => s.AssignmentId == id).Include(s => s.Student).ToListAsync();
}

public class GroupRepository(AppDbContext context) : IGroupRepository {
    public async Task AddGroupAsync(Group g) => await context.Groups.AddAsync(g);
    public async Task<Group?> GetByIdAsync(int id) => await context.Groups.Include(g => g.Students).FirstOrDefaultAsync(g => g.Id == id);
    public async Task<List<Group>> GetAllAsync() => await context.Groups.ToListAsync();
}