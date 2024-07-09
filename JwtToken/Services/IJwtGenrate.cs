using JwtToken.Models;

namespace JwtToken.Services
{
    public interface IJwtGenrate
    {
        bool NewUser(User user);
        bool UpdateUser(User user);
    }
}
