using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/projects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Include(p => p.Members)
                .ThenInclude(pm => pm.User)
            .ToListAsync();
    }

    // GET: api/projects/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(Guid id)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .Include(p => p.Members)
                .ThenInclude(pm => pm.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            return NotFound();

        return project;
    }

    // POST: api/projects
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(CreateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Status = dto.Status
        };

        // Add members
        foreach (var userId in dto.MemberIds)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                project.Members.Add(new ProjectMember
                {
                    Project = project,
                    User = user
                });
            }
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }



//   [HttpGet]
//     public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
//         {
//             var projects = await _context.Projects
//                 .Include(p => p.Tasks)
//                 .Include(p => p.Members)
//                     .ThenInclude(pm => pm.User)
//                 .ToListAsync();
    
//             return Ok(projects);
//         }



}
