namespace HomeworkTracker.Models;

public abstract class User {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class Student : User { }

public class Teacher : User { }

public class Assignment {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public int GroupId { get; set; }
    public bool IsExpired() => DateTime.Now > Deadline;
}

public class Submission {
    public int Id { get; set; }
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? Grade { get; set; }
    public string Status { get; set; } = "Нове";
}