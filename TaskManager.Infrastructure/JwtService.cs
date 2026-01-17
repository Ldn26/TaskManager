using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace TaskManager.Infrastructure   ; 


public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        _rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}



















// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Security.Cryptography;
// using System.Text;
// using Microsoft.Extensions.Configuration;

// namespace TaskManager.Infrastructure
// {
//     public class JwtService : IJwtService
//     {
//         private readonly IConfiguration _config;
//         private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

//         public JwtService(IConfiguration config)
//         {
//             _config = config ?? throw new ArgumentNullException(nameof(config));
//         }

//         public string GenerateAccessToken(User user)
//         {
//             var claims = new[]
//             {
//                 new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
//                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
//                 new Claim(ClaimTypes.Role, user.Role.ToString())
//             };

//             var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
//             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//             var token = new JwtSecurityToken(
//                 issuer: _config["Jwt:Issuer"],
//                 audience: _config["Jwt:Audience"],
//                 claims: claims,
//                 expires: DateTime.UtcNow.AddMinutes(15),
//                 signingCredentials: creds
//             );

//             return new JwtSecurityTokenHandler().WriteToken(token);
//         }

//         public string GenerateRefreshToken()
//         {
//             var bytes = new byte[64];
//             _rng.GetBytes(bytes);
//             return Convert.ToBase64String(bytes);
//         }
//     }
// }
