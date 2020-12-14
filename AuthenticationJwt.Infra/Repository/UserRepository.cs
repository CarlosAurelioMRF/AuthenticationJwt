using AuthenticationJwt.Core.Base;
using AuthenticationJwt.Core.Base.Interface;
using AuthenticationJwt.Domain.Entities;
using AuthenticationJwt.Domain.Interfaces.Repository;

namespace AuthenticationJwt.Infra.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseSettings settings) : base(settings)
        {
        }

        public bool UserExists(string username) => Count(c => c.Username == username) > 0;

    }
}
