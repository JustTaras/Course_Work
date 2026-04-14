namespace HomeworkTracker.Models;

public class Teacher : User
{
    // Зв'язок: один викладач має багато завдань
    public List<Assignment> Assignments { get; set; } = new();

    public override string GetInfo()
    {
        return $"{base.GetInfo()}, Роль: Викладач, Завдань: {Assignments.Count}";
    }
}