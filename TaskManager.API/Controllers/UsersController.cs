using Microsoft.AspNetCore.Mvc;
using TaskManager.Infrastructure;
namespace TaskManager.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public UsersController(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

 
    [HttpPost("register")]
    public ActionResult CreateUser(RegisterDto dto)
    {
        if (_context.Users.Any(u => u.Email == dto.Email))
            return Conflict("A user with the same email already exists.");

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 4)
            return BadRequest("Password must be at least 4 characters long.");

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            return BadRequest("Invalid role specified.");

        if (string.IsNullOrWhiteSpace(dto.FullName))
            return BadRequest("Full name is required.");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FullName = dto.FullName,
            Role = role
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("User created successfully");
    }


    [HttpPost("login")]
    public ActionResult LoginUser(LoginDto dto)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(new
        {
            message = "Login successful",
            user  = new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.Role
            }, 
            accessToken , 
        });
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // refresh
   
    [HttpPost("refresh")]
public ActionResult RefreshToken()
{
    var refreshToken = Request.Cookies["RefreshToken"];
    if (string.IsNullOrEmpty(refreshToken))
        return Unauthorized("No refresh token");

    var user = _context.Users.FirstOrDefault();
    if (user == null)
        return Unauthorized("User not found");

    var newAccessToken = _jwtService.GenerateAccessToken(user);

    Response.Cookies.Append("AccessToken", newAccessToken, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddMinutes(15)
    });

    return Ok(new { message = "Access token refreshed" });
}
}
