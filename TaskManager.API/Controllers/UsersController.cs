using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure;
namespace TaskManager.API.Controllers;
[ApiController] 
[Route("api/[controller]")]
public class UsersController : ControllerBase   { 
    private readonly AppDbContext _context;
    public UsersController(AppDbContext context)  {
        _context = context  ; 
    } 
        [HttpPost]
//    retrun the User in create User
  public ActionResult<User> CreateUser(RegisterDto dto)  
    { 
        if(_context.Users.Any(u=>u.Email == dto.Email))  
        { 
            return Conflict("A user with the same email already exists."); 
        }


 if(string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)  
        { 
            return BadRequest("Password must be at least 6 characters long."); 
        }


        if(!Enum.TryParse<UserRole>(dto.Role, true, out var role))  
        { 
            return BadRequest("Invalid role specified."); 
        }


        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            return BadRequest("Full name is required.");
        }

  var user = new User   {
        Email = dto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)   , 
          FullName = dto.FullName,
        Role =  Enum.Parse<UserRole>(dto.Role, true) 
    };
        _context.Users.Add(user); 
        _context.SaveChanges(); 
    return Ok("User created successfully" );
     } 







    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(int id)  
    { 
        var user = _context.Users.Find(id); 
        if (user == null)  
        { 
            return NotFound(); 
        } 
        return Ok(user); 
    }


        
}