using Base.Application.Commands;
using Base.Application.Services;
using Base.Domain.Tests.Helpers;
using Base.Infrastructure.Repositories;
using Base.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Tests.Services
{
    public class UserServiceDomainTests
    {
        [Fact]
        public async Task CanUpdatePassword_ForSeededUser()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var repo = new UserRepository(context);
            var hasher = new PasswordHasher();
            var role = new RoleRepository(context);
           // var service = new UserService(repo, hasher, role);

            var user = await context.Users.FirstAsync(u => u.UserName == "admin");
            var command = new ChangePasswordCommand(user.Id, "Admin123!", "NewPass456!");

            // Act
            //var response = await service.ChangePasswordAsync(command);

            // Assert
            //ssert.Equal("admin", response.Username);
            Assert.NotNull(user.PasswordChangedAt);
        }
    }
}
