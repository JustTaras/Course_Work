namespace HomeworkTracker.Interfaces;
using HomeworkTracker.Models;

public interface IAssignmentRepository {
    void Add(Assignment item);
    IEnumerable<Assignment> GetAll();
}

public interface ISubmissionRepository {
    void Add(Submission item);
    void Update(Submission item);
}

public interface IUnitOfWork {
    IAssignmentRepository Assignments { get; }
    ISubmissionRepository Submissions { get; }
}