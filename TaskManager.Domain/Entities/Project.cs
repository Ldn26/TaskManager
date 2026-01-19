public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ProjectStatus Status { get; set; }

// tasks 
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

// memeber
            public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();


}