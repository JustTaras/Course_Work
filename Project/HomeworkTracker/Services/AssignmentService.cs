using HomeworkTracker.Models;
using HomeworkTracker.Repositories;

namespace HomeworkTracker.Services;

public class AssignmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Assignment> CreateAssignment(string title, string description, DateTime deadline, int groupId, int teacherId)
    {
        var assignment = new Assignment
        {
            Title = title,
            Description = description,
            Deadline = deadline,
            GroupId = groupId,
            TeacherId = teacherId
        };

        await _unitOfWork.Assignments.AddAssignmentAsync(assignment);
        await _unitOfWork.CommitAsync();
        return assignment;
    }

    public async Task<List<Assignment>> GetActiveAssignments()
    {
        var all = await _unitOfWork.Assignments.GetAllAsync();
        return all.Where(a => !a.IsExpired()).ToList();
    }
}