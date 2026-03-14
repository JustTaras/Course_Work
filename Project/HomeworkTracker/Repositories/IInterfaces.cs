using HomeworkTracker.Models;

namespace HomeworkTracker.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<List<User>> GetAllUsersAsync();
}

public interface IAssignmentRepository
{
    Task AddAssignmentAsync(Assignment assignment);
    Task<Assignment?> GetByIdAsync(int id);
    Task<List<Assignment>> GetAllAsync();
}

public interface ISubmissionRepository
{
    Task AddSubmissionAsync(Submission submission);
    Task<Submission?> GetByIdAsync(int id);
    Task<List<Submission>> GetByAssignmentIdAsync(int assignmentId);
}

public interface IGroupRepository
{
    Task AddGroupAsync(Group group);
    Task<Group?> GetByIdAsync(int id);
    Task<List<Group>> GetAllAsync();
}