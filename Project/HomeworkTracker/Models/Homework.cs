namespace HomeworkTracker.Models;

public class Homework
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
    public bool IsCompleted { get; set; }
}