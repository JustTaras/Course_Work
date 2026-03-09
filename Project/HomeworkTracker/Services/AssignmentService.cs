namespace HomeworkTracker.Services;
using HomeworkTracker.Models;
using HomeworkTracker.Interfaces;

public class AssignmentService {
    private readonly IUnitOfWork _uow;
    public AssignmentService(IUnitOfWork uow) => _uow = uow;

    public void Create(string title) => 
        _uow.Assignments.Add(new Assignment { Title = title, Deadline = DateTime.Now.AddDays(7) });

    public List<Assignment> GetList() => _uow.Assignments.GetAll().ToList();
}