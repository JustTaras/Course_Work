using HomeworkTracker.Models;
using HomeworkTracker.Data;

namespace HomeworkTracker.Services
{
    public class SubmissionService
    {
        private readonly AppDbContext _context;
        public SubmissionService(AppDbContext context) { _context = context; }

        public List<Submission> GetList() => _context.Submissions.ToList();
        public void SubmitWork(int assignmentId, int studentId, string content)
        {
            _context.Submissions.Add(new Submission {
                AssignmentId = assignmentId, StudentId = studentId, Content = content,
                SubmissionDate = DateTime.Now, Status = "Очікує перевірки"
            });
            _context.SaveChanges();
        }
    }
}