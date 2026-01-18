
namespace TaskManager.Infrastructure   ; 
public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user); 
}
