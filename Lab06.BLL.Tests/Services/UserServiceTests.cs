using Lab06.BLL.Services;
using Lab06.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.BLL.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserStore<ApplicationUser> _userStoreMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private UserService _sut;

        [SetUp]
        public void Setup()
        {
            _userStoreMock = Mock.Of<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(_userStoreMock, null, null, null, null, null, null, null, null);
            _sut = new UserService(_userManagerMock.Object);
        }

        [Test]
        public async Task GetAllUsersWithUserRoleAsync_ShouldReturnNull_WhenUsersDoesNotExists()
        {
            // Arrange
            _userManagerMock.Setup(x => x.Users)
                .Returns(() => null);

            // Act
            var actualResult = await _sut.GetAllUsersWithUserRoleAsync();

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        public async Task GetAllUsersWithUserRoleAsync_ShouldReturnEmpty_WhenUsersInUserRoleDoesNotExists()
        {
            // Arrange
            var applicationUser = new ApplicationUser
            {
                Id = "Test",
            };

            _userManagerMock.Setup(x => x.Users)
                .Returns(Queryable.Append(new List<ApplicationUser>().AsQueryable(), applicationUser));

            _userManagerMock.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => false);
            // Act
            var actualResult = await _sut.GetAllUsersWithUserRoleAsync();

            // Assert
            _userManagerMock.Verify(_ => _.Users, Times.Once);
            _userManagerMock.Verify(_ => _.IsInRoleAsync(It.IsAny<ApplicationUser>(), "User"), Times.AtLeastOnce);
            Assert.IsEmpty(actualResult);
        }
    }
}
