namespace HomeworkTracker.Repositories;

using HomeworkTracker.Models;
using HomeworkTracker.Interfaces;

public class AssignmentRepo : IAssignmentRepository {
    private List<Assignment> _data = new();
    public void Add(Assignment item) { item.Id = _data.Count + 1; _data.Add(item); }
    public IEnumerable<Assignment> GetAll() => _data;
    public Assignment? GetById(int id) => _data.FirstOrDefault(a => a.Id == id);
}

public class SubmissionRepo : ISubmissionRepository {
    private List<Submission> _submissions = new();
    public void Add(Submission item) { item.Id = _submissions.Count + 1; _submissions.Add(item); }
    public IEnumerable<Submission> GetByAssignment(int assignmentId) => _submissions.Where(s => s.AssignmentId == assignmentId);
    public void Update(Submission item) {
        var existing = _submissions.FirstOrDefault(s => s.Id == item.Id);
        if (existing != null) { existing.Grade = item.Grade; existing.Status = item.Status; }
    }
    public Submission? GetById(int id) => _submissions.FirstOrDefault(s => s.Id == id);
}