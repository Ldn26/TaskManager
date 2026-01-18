using Microsoft.AspNetCore.Mvc;
using TaskManager.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System;

namespace TaskManager.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

private readonly IConfiguration _config;

public UsersController(AppDbContext context, IJwtService jwtService, IConfiguration config)
{
    _context = context;
    _jwtService = jwtService;
    _config = config;
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
                Id = Guid.NewGuid(),
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
        var refreshToken = _jwtService.GenerateRefreshToken(user);
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




[HttpPost("refresh")]
public ActionResult RefreshToken()
{
    var refreshToken = Request.Cookies["RefreshToken"];
    if (string.IsNullOrEmpty(refreshToken))
        return Unauthorized("No refresh token");

    try
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

        // Validate the refresh token
        tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;

        // Extract user ID from the refresh token
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
            return Unauthorized("Invalid refresh token");

        var userId = Guid.Parse(userIdClaim.Value); // your User Id is a Guid
        var user = _context.Users.Find(userId);
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
    catch
    {
        return Unauthorized("Invalid refresh token");
    }
}


}
