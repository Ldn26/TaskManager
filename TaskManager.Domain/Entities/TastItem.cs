public class TaskItem
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required TaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime DueDate { get; set; }
}
