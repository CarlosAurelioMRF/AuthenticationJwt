using AuthenticationJwt.Domain.Entities;
using AuthenticationJwt.Domain.Interfaces.Repository;
using AuthenticationJwt.Services.Authorization;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Linq.Expressions;
using Xunit;

namespace AuthenticationJwt.Services.Tests.AuthorizationTests
{
    [Collection(nameof(AuthCollection))]
    public class AuthServiceTests
    {
        private readonly AuthFixtureTests _authFixtureTests;
        private readonly AuthService _authService;

        public AuthServiceTests(AuthFixtureTests authFixtureTests)
        {
            _authFixtureTests = authFixtureTests;
            _authService = _authFixtureTests.GetService();
        }

        [Fact]
        public void Auth_Register_ShouldBeValid()
        {
            // Arrange
            var userForRegisterDto = _authFixtureTests.GetUserForRegister();

            // Act
            var userId = _authService.Register(userForRegisterDto);

            // Assert
            Assert.NotNull(userId);
            _authFixtureTests.Mocker.GetMock<IUserRepository>().Verify(b => b.Insert(It.IsAny<User>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Auth_Login_ShouldBeValid()
        {
            // Arrange
            var expected = _authFixtureTests.GetListUsers();
            var userForLoginDto = _authFixtureTests.GetUserForLogin(expected[new Random().Next(expected.Count)]);

            _authFixtureTests.Mocker.GetMock<IUserRepository>().Setup(moq => moq.GetAll(It.IsAny<Expression<Func<User, bool>>>())).Returns(expected);
            _authFixtureTests.Mocker.GetMock<IConfiguration>().Setup(c => c.GetSection("AppSettings:Token").Value).Returns(_authFixtureTests.RandomToken());

            // Act
            var accessToken = _authService.Login(userForLoginDto);

            // Assert
            Assert.NotNull(accessToken);
        }
    }
}
