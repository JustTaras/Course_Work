namespace HomeworkTracker.Repositories;
using HomeworkTracker.Models;
using HomeworkTracker.Interfaces;

public class AssignmentRepo : IAssignmentRepository {
    private readonly List<Assignment> _data = new();
    public void Add(Assignment item) { item.Id = _data.Count + 1; _data.Add(item); }
    public IEnumerable<Assignment> GetAll() => _data;
}

public class SubmissionRepo : ISubmissionRepository {
    private readonly List<Submission> _data = new();
    public void Add(Submission item) { item.Id = _data.Count + 1; _data.Add(item); }
    public void Update(Submission item) { /* Логіка оновлення */ }
}

public class UnitOfWork : IUnitOfWork {
    public IAssignmentRepository Assignments { get; } = new AssignmentRepo();
    public ISubmissionRepository Submissions { get; } = new SubmissionRepo();
}