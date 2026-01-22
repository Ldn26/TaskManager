using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.AssignedUser)
            .ToListAsync();
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(Guid id)
    {
        var task = await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.AssignedUser)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            return NotFound();

        return task;
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(TaskDto dto)
    {
        // Validate required fields
        if (string.IsNullOrEmpty(dto.Title))
            return BadRequest("Title is required");

        if (dto.ProjectId == null)
            return BadRequest("ProjectId is required");

        var project = await _context.Projects.FindAsync(dto.ProjectId.Value);
        if (project == null)
            return NotFound("Project not found");

        // Optional: check if assigned user is part of project
        if (dto.AssignedUserId.HasValue)
        {
            bool isMember = await _context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == dto.ProjectId.Value && pm.UserId == dto.AssignedUserId.Value);

            if (!isMember)
                return BadRequest("Assigned user is not a member of the project");
        }

        var task = new TaskItem
        {
            Title = dto.Title!,
            Status = dto.Status ?? TaskStatus.Todo,       // default if null
            Priority = dto.Priority ?? Priority.Medium,   // default if null
            DueDate = dto.DueDate ?? DateTime.UtcNow,     // default if null
            ProjectId = dto.ProjectId.Value,
            AssignedUserId = dto.AssignedUserId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, TaskDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        // Update only if value is provided
        if (!string.IsNullOrEmpty(dto.Title))
            task.Title = dto.Title;

        if (dto.Status != null)
            task.Status = dto.Status.Value;

        if (dto.Priority != null)
            task.Priority = dto.Priority.Value;

        if (dto.DueDate != null)
            task.DueDate = dto.DueDate.Value;

        if (dto.ProjectId != null)
            task.ProjectId = dto.ProjectId.Value;

        if (dto.AssignedUserId != null)
        {
            // Determine ProjectId to check: new one if provided, else current
            Guid projectIdToCheck = dto.ProjectId ?? task.ProjectId;

            bool isMember = await _context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == projectIdToCheck && pm.UserId == dto.AssignedUserId.Value);

            if (!isMember)
                return BadRequest("Assigned user is not a member of the project");

            task.AssignedUserId = dto.AssignedUserId;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }







[HttpPut("updateStatus/{id}")]
public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] TaskStatusUpdateDto dto)
{
    var task = await _context.Tasks.FindAsync(id);
    if (task == null)
        return NotFound();

    task.Status = dto.Status;
    await _context.SaveChangesAsync();

    return NoContent();
}

























  


[HttpGet("getTaskByStatus")]
public async Task<ActionResult<IEnumerable<TaskByStatusDto>>> GetTasksByStatus(
    [FromQuery] TaskStatus status)
{
    var tasks = await _context.Tasks
        .Where(t => t.Status == status)
        .Select(t => new TaskByStatusDto
        {
            Id = t.Id,
            Title = t.Title,
            Status = t.Status,
            ProjectId = t.Project.Id,
            ProjectName = t.Project.Name
        })
        .ToListAsync();

    return Ok(tasks);
}
    [HttpGet("nbrOfTasks")]    public async Task<ActionResult<int>> GetNumberOfTasks()
    {
        int count = await _context.Tasks.CountAsync();
        return count;
    }





[HttpGet("tasksByUser/{userId}")]
public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByUser(Guid userId)
{
    var tasks = await _context.Tasks
        .Where(t => t.AssignedUserId == userId)
        .Include(t => t.Project)
        .Include(t => t.AssignedUser)
        .ToListAsync();
    return Ok(tasks);
}


}

