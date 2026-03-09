namespace HomeworkTracker.Interfaces;
using HomeworkTracker.Models;

public interface IAssignmentRepository {
    void Add(Assignment item);
    IEnumerable<Assignment> GetAll();
    Assignment? GetById(int id);
}

public interface ISubmissionRepository {
    void Add(Submission item);
    IEnumerable<Submission> GetByAssignment(int assignmentId);
    void Update(Submission item);
    Submission? GetById(int id);
}

public interface IUnitOfWork {
    IAssignmentRepository Assignments { get; }
    ISubmissionRepository Submissions { get; }
    void Commit();
}