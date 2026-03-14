namespace HomeworkTracker.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IAssignmentRepository Assignments { get; }
    ISubmissionRepository Submissions { get; }
    IGroupRepository Groups { get; }
    Task<int> CommitAsync();
}