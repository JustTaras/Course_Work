namespace HomeworkTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class Student : User
    {
        public int GroupId { get; set; }
        public Group? Group { get; set; }
    }

    public class Teacher : User
    {
        
    }
}