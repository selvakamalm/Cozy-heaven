using DAL.Models.Main;

namespace ServiceLayer.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
