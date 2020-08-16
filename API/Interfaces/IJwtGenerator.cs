
namespace API.Interfaces
{
    public interface IJwtGenerator
    {
         string CreateToken(Entities.AppUser user);
    }
}