namespace HomeworkTracker.Repositories;

using HomeworkTracker.Interfaces;

public class UnitOfWork : IUnitOfWork {
    public IAssignmentRepository Assignments { get; } = new AssignmentRepo();
    public ISubmissionRepository Submissions { get; } = new SubmissionRepo();
    
    public void Commit() { 
    
    }
}