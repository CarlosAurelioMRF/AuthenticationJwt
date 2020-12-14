using AuthenticationJwt.Domain.Dtos;
using AuthenticationJwt.Domain.Entities;
using AuthenticationJwt.Domain.Interfaces.Repository;
using AuthenticationJwt.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AuthenticationJwt.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public UserDto GetUser(string id)
        {
            var user = _userRepository.GetById(id);

            if (user == null) return null;

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Deleted = user.DeletedAt.HasValue
            };

            return userDto;
        }

        public string Login(UserForLoginDto userForLogin)
        {
            var user = _userRepository.GetAll(u => u.Username == userForLogin.Username.ToLower()).FirstOrDefault();

            if (user == null) return null;

            if (!VerifyPasswordHash(userForLogin.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return GenerateAccessToken(user);
        }

        public string Register(UserForRegisterDto userForRegisterDto)
        {
            if (userForRegisterDto == null) throw new Exception("User invalid.");

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (_userRepository.UserExists(userForRegisterDto.Username)) throw new Exception("Username exist");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                CreatedAt = DateTime.Now
            };

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

            userToCreate.PasswordHash = passwordHash;
            userToCreate.PasswordSalt = passwordSalt;

            _userRepository.Insert(userToCreate);

            return userToCreate.Id;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                    if (computedHash[i] != passwordHash[i]) return false;
            }

            return true;
        }

        private string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
