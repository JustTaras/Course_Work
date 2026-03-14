namespace HomeworkTracker.Models;

public abstract class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public virtual string GetInfo()
    {
        return $"ID: {Id}, Name: {Name}, Email: {Email}";
    }
}