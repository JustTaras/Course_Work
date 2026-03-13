namespace HomeworkTracker.Services;
using HomeworkTracker.Models;
using HomeworkTracker.Interfaces;

public class AssignmentService {
    private readonly IUnitOfWork _uow;
    public AssignmentService(IUnitOfWork uow) => _uow = uow;

public void Create(string title, string description, string groupName, DateTime deadline)
{
    _uow.Assignments.Add(new Assignment 
    { 
        Title = title, 
        Description = description,
        GroupName = groupName,
        Deadline = deadline 
    });
}
    public List<Assignment> GetList() => _uow.Assignments.GetAll().ToList();
}