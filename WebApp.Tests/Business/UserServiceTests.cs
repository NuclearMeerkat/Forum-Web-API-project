using AutoMapper;
using FluentAssertions;
using Moq;
using WebApp.BusinessLogic.Services;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
using WebApp.Core.Enums;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Models;

namespace WebApp.Tests.Business;

public class UserServiceTests
{
    private Mock<IUnitOfWork> mockUnitOfWork;
        private IMapper mapper;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mapper = UnitTestBusinessHelper.CreateMapperProfile();
            userService = new UserService(mockUnitOfWork.Object, mapper);
        }

        [Test]
        public async Task GetAllAsyncReturnsAllUsers()
        {
            // Arrange
            var expectedUsers = GetTestUserModels();
            mockUnitOfWork
                .Setup(u => u.UserRepository.GetAllAsync())
                .ReturnsAsync(GetTestUserEntities());

            // Act
            var actualUsers = await userService.GetAllAsync();

            // Assert
            actualUsers.Should().BeEquivalentTo(expectedUsers);
        }

        [Test]
        public async Task GetByIdAsyncReturnsCorrectUserModel()
        {
            // Arrange
            var expectedUser = GetTestUserModels().First();
            int userId = expectedUser.Id;
            mockUnitOfWork
                .Setup(u => u.UserRepository.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync(GetTestUserEntities().First());

            // Act
            var actualUser = await userService.GetByIdAsync(userId);

            // Assert
            actualUser.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task AddAsyncAddsUser()
        {
            // Arrange
            var createModel = new UserCreateModel
            {
                Nickname = "NewUser",
                Email = "newuser@example.com",
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            mockUnitOfWork.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>()));
            // Act
            await userService.AddAsync(createModel);

            // Assert
            mockUnitOfWork.Verify(u => u.UserRepository.AddAsync(It.Is<User>(
                user => user.Nickname == createModel.Nickname &&
                        user.Email == createModel.Email &&
                        user.Role == createModel.Role &&
                        user.CreatedAt == createModel.CreatedAt)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task AddAsyncThrowsExceptionWhenCreateModelIsInvalid()
        {
            // Arrange
            var invalidCreateModel = new UserCreateModel { Nickname = string.Empty, Email = "" };

            // Act
            Func<Task> act = async () => await userService.AddAsync(invalidCreateModel);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task UpdateAsyncUpdatesUser()
        {
            // Arrange
            var userModel = new UserCreateModel()
            {
                Nickname = "UpdatedUser",
                Email = "updateduser@example.com",
            };

            mockUnitOfWork.Setup(u => u.UserRepository.Update(It.IsAny<User>()));

            // Act
            await userService.UpdateAsync(userModel);

            // Assert
            mockUnitOfWork.Verify(u => u.UserRepository.Update(It.Is<User>(
                user =>
                        user.Nickname == userModel.Nickname &&
                        user.Email == userModel.Email)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsyncDeletesUser()
        {
            // Arrange
            int userId = 1;
            mockUnitOfWork.Setup(u => u.UserRepository.DeleteByIdAsync(It.IsAny<int>()));

            // Act
            await userService.DeleteAsync(userId);

            // Assert
            mockUnitOfWork.Verify(u => u.UserRepository.DeleteByIdAsync(userId), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        // Helper methods for test data
        private List<UserModel> GetTestUserModels()
        {
            return new List<UserModel>
            {
                new UserModel
                {
                    Id = 1,
                    Nickname = "User1",
                    Email = "user1@example.com",
                    Role = UserRole.User,
                    CreatedAt = DateTime.Today,
                    LastLogin = DateTime.Today
                },
                new UserModel
                {
                    Id = 2,
                    Nickname = "User2",
                    Email = "user2@example.com",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.Today,
                    LastLogin = DateTime.Today
                }
            };
        }

        private List<User> GetTestUserEntities()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Nickname = "User1",
                    Email = "user1@example.com",
                    Role = UserRole.User,
                    CreatedAt = DateTime.Today,
                    LastLogin = DateTime.Today
                },
                new User
                {
                    Id = 2,
                    Nickname = "User2",
                    Email = "user2@example.com",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.Today,
                    LastLogin = DateTime.Today
                }
            };
        }
}
