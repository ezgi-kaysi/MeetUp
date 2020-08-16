using API.Dtos;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IFacebookAccessor
    {
         Task<FacebookUserInfo> FacebookLogin(string accessToken);
    }
}