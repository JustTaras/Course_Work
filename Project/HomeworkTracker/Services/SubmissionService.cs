using HomeworkTracker.Models;
using HomeworkTracker.Interfaces;

namespace HomeworkTracker.Services
{
    public class SubmissionService
    {
        private readonly IUnitOfWork _uow;

        public SubmissionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void SubmitWork(int assignmentId, string studentName, string content)
{
    var submission = new Submission
    {
        AssignmentId = assignmentId,
        Content = content,
        Status = "Очікує перевірки"
    };
    _uow.Submissions.Add(submission);
}
public void GradeSubmission(int submissionId, int grade)
{
    var submission = GetSubmissions().FirstOrDefault(s => s.Id == submissionId);
    
    if (submission != null)
    {
        submission.Grade = grade;
        submission.Status = "Перевірено";
    }
}

        public List<Submission> GetSubmissions()
{
    return _uow.Submissions.GetAll().ToList();
}
    }
}