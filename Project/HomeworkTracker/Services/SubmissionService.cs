using HomeworkTracker.Models;
using HomeworkTracker.Repositories;

namespace HomeworkTracker.Services;

public class SubmissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public SubmissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Submission> Submit(int assignmentId, int studentId, string content)
    {
        var submission = new Submission
        {
            AssignmentId = assignmentId,
            StudentId = studentId,
            Content = content,
            SubmitDate = DateTime.UtcNow,
            Status = "Submitted"
        };

        await _unitOfWork.Submissions.AddSubmissionAsync(submission);
        await _unitOfWork.CommitAsync();
        return submission;
    }

    public async Task Grade(int submissionId, int grade)
    {
        var submission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        if (submission == null) return;

        var assignment = await _unitOfWork.Assignments.GetByIdAsync(submission.AssignmentId);
        int maxGrade = assignment?.GradingScale ?? 100;
        grade = Math.Clamp(grade, 1, maxGrade);

        submission.SetGrade(grade);
        await _unitOfWork.CommitAsync();
    }
}