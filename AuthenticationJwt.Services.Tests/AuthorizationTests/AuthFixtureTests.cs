using AuthenticationJwt.Domain.Dtos;
using AuthenticationJwt.Domain.Entities;
using AuthenticationJwt.Services.Authorization;
using Bogus;
using MongoDB.Bson;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AuthenticationJwt.Services.Tests.AuthorizationTests
{
    [CollectionDefinition(nameof(AuthCollection))]
    public class AuthCollection : ICollectionFixture<AuthFixtureTests>
    {
    }

    public class AuthFixtureTests : IDisposable
    {
        private static readonly string PasswordTest = "?h%FQg3}A8RnDj~";

        public AuthService AuthService;
        public AutoMocker Mocker;

        public void Dispose()
        {
        }

        public AuthService GetService()
        {
            Mocker = new AutoMocker();
            AuthService = Mocker.CreateInstance<AuthService>();

            return AuthService;
        }

        public string RandomToken() => new Faker().Random.AlphaNumeric(30);

        public UserForRegisterDto GetUserForRegister()
        {
            return new Faker<UserForRegisterDto>("pt_BR")
                .CustomInstantiator(f => new UserForRegisterDto()
                {
                    Username = f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()),
                    Email = f.Internet.Email(),
                    Password = f.Internet.Password(20)
                })
                .Generate(1)
                .FirstOrDefault();
        }

        public UserForLoginDto GetUserForLogin(User user)
        {
            var userForLoginDto = new UserForLoginDto()
            {
                Username = user.Username,
                Password = PasswordTest
            };

            return userForLoginDto;
        }

        public List<User> GetListUsers()
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(out passwordHash, out passwordSalt);

            return new Faker<User>("pt_BR")
                .CustomInstantiator(f => new User()
                {
                    Id = f.Random.Guid().ToString(),
                    Username = f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()),
                    Email = f.Internet.Email(),
                    CreatedAt = f.Date.Soon(),
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash
                })
                .Generate(5);
        }

        private void CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();

            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(PasswordTest));
        }
    }
}
