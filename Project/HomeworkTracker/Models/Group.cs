namespace HomeworkTracker.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Зв'язок: одна група має багато студентів
    public List<Student> Students { get; set; } = new();
    
    // Зв'язок: одній групі дають багато завдань
    public List<Assignment> Assignments { get; set; } = new();

    public void AddStudent(Student student)
    {
        if (!Students.Contains(student))
            Students.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        Students.Remove(student);
    }
}