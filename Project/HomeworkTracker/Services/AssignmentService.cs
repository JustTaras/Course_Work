namespace HomeworkTracker.Services;

using HomeworkTracker.Models;
using HomeworkTracker.Interfaces;
using HomeworkTracker.Repositories;

public class AssignmentService {
    private readonly IUnitOfWork _uow;
    public AssignmentService(IUnitOfWork uow) => _uow = uow;

    public void CreateAssignment(string title, DateTime deadline) {
        _uow.Assignments.Add(new Assignment { Title = title, Deadline = deadline });
    }

    public List<Assignment> GetAll() => _uow.Assignments.GetAll().ToList();
}