namespace HomeworkTracker.Interfaces;
using HomeworkTracker.Models;

public interface IAssignmentRepository {
    void Add(Assignment item);
    IEnumerable<Assignment> GetAll();
}

public interface ISubmissionRepository {
    void Add(Submission item);
    void Update(Submission item);
    Submission? GetById(int id);
    IEnumerable<Submission> GetByAssignment(int assignmentId);
}

public interface IUnitOfWork {
    IAssignmentRepository Assignments { get; }
    ISubmissionRepository Submissions { get; }
}