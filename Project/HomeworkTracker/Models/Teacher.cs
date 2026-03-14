namespace HomeworkTracker.Models;

public class Teacher : User
{
    // Зв'язок: один викладач має багато завдань
    public List<Assignment> Assignments { get; set; } = new();
}