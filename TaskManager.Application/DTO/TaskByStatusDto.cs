public class TaskByStatusDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TaskStatus Status { get; set; }

    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
}
