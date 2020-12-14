using AuthenticationJwt.Domain.Dtos;
using System.Threading.Tasks;

namespace AuthenticationJwt.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        UserDto GetUser(string id);
        string Register(UserForRegisterDto userForRegisterDto);
        string Login(UserForLoginDto userForLogin);
    }
}
