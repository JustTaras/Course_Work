namespace HomeworkTracker.Models;

public class Assignment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    
    // До якої групи належить
    public int GroupId { get; set; }
    public Group? Group { get; set; }

    // Хто створив (Викладач)
    public int TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    // Зв'язок з відправленими роботами
    public List<Submission> Submissions { get; set; } = new();

    public bool IsExpired()
    {
        return DateTime.Now > Deadline;
    }

    public string GetInfo()
    {
        return $"[{Title}] {Description} (Deadline: {Deadline.ToString("dd.MM.yyyy")})";
    }
}