namespace HomeworkTracker.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Content { get; set; } = string.Empty;
        public int? Grade { get; set; }
        public string Status { get; set; } = "Надіслано";

    }
}