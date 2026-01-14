using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure;

namespace TaskManager.API.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class TasksController : ControllerBase
// {
//     private readonly AppDbContext _context;

//     public TasksController(AppDbContext context)
//     {
//         _context = context;
//     }

//     // GET: api/tasks
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
//     {
//         return await _context.Tasks.ToListAsync();
//     }

//     // GET: api/tasks/{id}
//     [HttpGet("{id}")]
//     public async Task<ActionResult<TaskItem>> GetTask(Guid id)
//     {
//         var task = await _context.Tasks.FindAsync(id);

//         if (task == null)
//             return NotFound();

//         return task;
//     }

//     // POST: api/tasks
//     [HttpPost]
//     public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
//     {
//         _context.Tasks.Add(task);
//         await _context.SaveChangesAsync();

//         return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
//     }

//     // PUT: api/tasks/{id}
//     [HttpPut("{id}")]
//     public async Task<IActionResult> UpdateTask(Guid id, TaskItem task)
//     {
//         if (id != task.Id)
//             return BadRequest();

//         _context.Entry(task).State = EntityState.Modified;
//         await _context.SaveChangesAsync();

//         return NoContent();
//     }

//     // DELETE: api/tasks/{id}
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteTask(Guid id)
//     {
//         var task = await _context.Tasks.FindAsync(id);
//         if (task == null)
//             return NotFound();

//         _context.Tasks.Remove(task);
//         await _context.SaveChangesAsync();

//         return NoContent();
//     }
// }
[ApiController]
// to give asp.dotnet the ability to take the controller Name TasksController     & make it tolowercase 
//   and this this the routename  
// like app.use("/api/tasks", tasksRouter);  in node 

[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    // it's dependency injection   to  make the contoller use the db  of the program.cs   not to create a new Interfaces\ 
    // so  “Don’t create what you need. Ask for it.”   wchrayk kho
    private readonly AppDbContext _context;


    public TasksController(AppDbContext context)  {_context = context;   }

    [HttpGet]
    // IEnumerable<TaskItem> → array/list of tasks
    // ActionResult → allows returning different HTTP responses (200 OK, 404 NotFound, etc.)
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        return await _context.Tasks.ToListAsync();
    }    
}
