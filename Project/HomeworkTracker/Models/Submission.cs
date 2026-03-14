namespace HomeworkTracker.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; } 
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = "Очікує перевірки"; 
        public int? Grade { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
    }
}