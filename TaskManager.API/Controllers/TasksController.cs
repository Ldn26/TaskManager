

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// // to give asp.dotnet the ability to take the controller Name TasksController 
// //    not the suffix      the route nae will be Tasks  & make it tolowercase 
// //   and this this the routename  
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;
//     // it's dependency injection   to  make the contoller use the db  of the program.cs   not to create a new Interfaces\ 
//     // so  “Don’t create what you need. Ask for it.”   wchrayk kho
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
        var project = await _context.Projects.FindAsync(dto.ProjectId);
        if (project == null)
            return NotFound("Project not found");

        // Optional: check if assigned user is part of project
        if (dto.AssignedUserId.HasValue)
        {
            bool isMember = await _context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == dto.ProjectId && pm.UserId == dto.AssignedUserId.Value);

            if (!isMember)
                return BadRequest("Assigned user is not a member of the project");
        }

        var task = new TaskItem
        {
            Title = dto.Title,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            ProjectId = dto.ProjectId,
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
        if (task == null) return NotFound();

        task.Title = dto.Title;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
        task.AssignedUserId = dto.AssignedUserId;

        // Optional: check assigned user is part of project
        if (dto.AssignedUserId.HasValue)
        {
            bool isMember = await _context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == task.ProjectId && pm.UserId == dto.AssignedUserId.Value);

            if (!isMember)
                return BadRequest("Assigned user is not a member of the project");
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
}
