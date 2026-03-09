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

    public void Add(Submission item) {
        item.Id = _data.Count + 1;
        _data.Add(item);
    }

    public void Update(Submission item) {
        var existing = GetById(item.Id);
        if (existing != null) {
            existing.Grade = item.Grade;
            existing.Status = item.Status;
        }
    }

    public Submission? GetById(int id) => _data.FirstOrDefault(s => s.Id == id);

    public IEnumerable<Submission> GetByAssignment(int assignmentId) {
        if (assignmentId == 0) return _data; // Для вчителя повертаємо все
        return _data.Where(s => s.AssignmentId == assignmentId);
    }
}

public class UnitOfWork : IUnitOfWork {
    public IAssignmentRepository Assignments { get; } = new AssignmentRepo();
    public ISubmissionRepository Submissions { get; } = new SubmissionRepo();
}