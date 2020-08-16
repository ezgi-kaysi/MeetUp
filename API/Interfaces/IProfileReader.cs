using API.Dtos;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IProfileReader
    {
         Task<Profile> ReadProfile(string username);
    }
}