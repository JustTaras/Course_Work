namespace HomeworkTracker.Models;

public class Submission
{
    public int Id { get; set; }
    
    // Яке завдання виконується
    public int AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }

    // Хто виконав
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public string Content { get; set; } = string.Empty;
    public DateTime SubmitDate { get; set; } = DateTime.Now;
    
    public int? Grade { get; set; } // Nullable, бо спочатку оцінки немає
    public string Status { get; set; } = "Submitted"; // Submitted, Checked, Graded

    public void SetGrade(int grade)
    {
        Grade = grade;
        Status = "Graded";
    }

    public void ChangeStatus(string status)
    {
        Status = status;
    }
}