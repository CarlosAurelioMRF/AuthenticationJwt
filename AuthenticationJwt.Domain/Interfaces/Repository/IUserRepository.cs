using AuthenticationJwt.Core.Base.Interface;
using AuthenticationJwt.Domain.Entities;

namespace AuthenticationJwt.Domain.Interfaces.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        bool UserExists(string username);
    }
}
