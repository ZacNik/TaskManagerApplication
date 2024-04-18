namespace TaskManagerApplication.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }

}
