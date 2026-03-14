namespace HomeworkTracker.Models;

public class Student : User
{
    // Зв'язок з групою
    public int? GroupId { get; set; } // Може бути null, якщо ще не в групі
    public Group? Group { get; set; }

    // Зв'язок: один студент має багато відправлених рішень
    public List<Submission> Submissions { get; set; } = new();
}