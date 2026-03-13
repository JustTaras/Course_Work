namespace HomeworkTracker.Services;
using HomeworkTracker.Models;
using HomeworkTracker.Interfaces;

public class SubmissionService {
    private readonly IUnitOfWork _uow;
    public SubmissionService(IUnitOfWork uow) => _uow = uow;

    public void SubmitWork(int assignmentId, string studentName, string content) {
        var submission = new Submission {
            AssignmentId = assignmentId,
            Content = content,
            Status = "Очікує перевірки"
        };
        _uow.Submissions.Add(submission);
    }

    public void GradeWork(int submissionId, int grade) {
        var sub = _uow.Submissions.GetById(submissionId);
        if (sub != null) {
            sub.Grade = grade;
            sub.Status = "Перевірено";
            _uow.Submissions.Update(sub);
        }
    }

    public List<Submission> GetAllSubmissions() => 
        _uow.Submissions.GetByAssignment(0).ToList();
}