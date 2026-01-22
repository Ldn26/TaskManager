public class TaskDto
{
    public string? Title { get; set; }
    public TaskStatus? Status { get; set; }
    public Priority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? AssignedUserId { get; set; }
}



public class CreateProjectDto
{
    public string Name { get; set; } = null!;
    public ProjectStatus Status { get; set; }
    public List<Guid> MemberIds { get; set; } = new List<Guid>();
}



