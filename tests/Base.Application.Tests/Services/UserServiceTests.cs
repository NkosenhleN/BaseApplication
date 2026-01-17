using Base.Application.Commands;
using Base.Application.Interfaces;
using Base.Application.Services;
using Base.Domain.Entities;
using Base.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPasswordHasher> _hasherMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _hasherMock = new Mock<IPasswordHasher>();
            _userService = new UserService(_userRepoMock.Object, _hasherMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_UserName_Exists()
        {
            // Arrange
            var existingUsername = "testuser";
            _userRepoMock.Setup(r => r.UsernameExistsAsync(existingUsername))
                    .ReturnsAsync(true);

            var service = new UserService(_userRepoMock.Object, _hasherMock.Object);

            var command = new CreateUserCommand(
                existingUsername,
                "John",
                "Doe",
                "john@example.com",
                "Password123!"
            );

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateUserAsync(command)
            );

            Assert.Equal("Username already exists", exception.Message);
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_Should_CreateUser_When_UsernameDoesNotExist()
        {
            // Arrange
            _userRepoMock.Setup(r => r.UsernameExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            _hasherMock.Setup(h => h.HashPassword(It.IsAny<string>()))
                       .Returns((new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }));

            var command = new CreateUserCommand(
                "newuser",
                "Jane",
                "Doe",
                "jane@example.com",
                "Password123!"
            );

            // Act
            var result = await _userService.CreateUserAsync(command);

            // Assert
            _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u =>
                u.UserName == "newuser" &&
                u.FirstName == "Jane" &&
                u.PasswordHash.Length == 3 &&
                u.PasswordSalt.Length == 3
            )), Times.Once);

            Assert.Equal("newuser", result.Username);
            Assert.Equal("Jane", result.FirstName);
        }

        [Fact]
        public async Task CreateUserAsync_Should_UsePasswordHasher()
        {
            // Arrange
            _userRepoMock.Setup(r => r.UsernameExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            var called = false;
            _hasherMock.Setup(h => h.HashPassword("Password123!"))
                       .Callback(() => called = true)
                       .Returns((new byte[1], new byte[1]));

            var command = new CreateUserCommand(
                "userhash",
                "Alice",
                "Smith",
                "alice@example.com",
                "Password123!"
            );

            // Act
            var result = await _userService.CreateUserAsync(command);

            // Assert
            Assert.True(called, "PasswordHasher.HashPassword was not called");
        }

        [Fact]
        public async Task CreateUserAsync_Should_MapEntityToResponseCorrectly()
        {
            // Arrange
            _userRepoMock.Setup(r => r.UsernameExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            _hasherMock.Setup(h => h.HashPassword(It.IsAny<string>()))
                       .Returns((new byte[] { 1 }, new byte[] { 1 }));

            var command = new CreateUserCommand(
                "mapuser",
                "Bob",
                "Johnson",
                "bob@example.com",
                "Password123!"
            );

            // Act
            var response = await _userService.CreateUserAsync(command);

            // Assert
            Assert.Equal("mapuser", response.Username);
            Assert.Equal("Bob", response.FirstName);
            Assert.Equal("Johnson", response.LastName);
        }

        [Fact]
        public async Task UpdatePasswordAsync_Should_Update_When_CurrentPasswordCorrect()
        {
            // Arrange
            var user = new User("user2", "Test", "User", "test@example.com", new byte[] { 1 }, new byte[] { 1 });
            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _hasherMock.Setup(h => h.VerifyPassword("CurrentPass", user.PasswordHash, user.PasswordSalt)).Returns(true);
            _hasherMock.Setup(h => h.HashPassword("NewPass123!")).Returns((new byte[] { 2 }, new byte[] { 2 }));

            var command = new ChangePasswordCommand(user.Id, "CurrentPass", "NewPass123!");

            // Act
            var response = await _userService.ChangePasswordAsync(command);

            // Assert
            _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
                u.PasswordHash.SequenceEqual(new byte[] { 2 }) &&
                u.PasswordSalt.SequenceEqual(new byte[] { 2 })
            )), Times.Once);

            Assert.Equal("user2", response.Username);
        }

    }
}
