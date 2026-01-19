public class TaskItem
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required TaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime DueDate { get; set; }


    // Link to project
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    // Optional: assign a user to the task
    public Guid? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }


}
