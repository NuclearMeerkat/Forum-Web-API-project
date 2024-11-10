using AutoMapper;
using FluentAssertions;
using Moq;
using WebApp.BusinessLogic.Services;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure;
using WebApp.Infrastructure.Auth;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Interfaces.Auth;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.Tests.Business;

public class UserServiceTests
{
    private Mock<IUnitOfWork> mockUnitOfWork;
        private IMapper mapper;
        private UserService userService;
        private IPasswordHasher passwordHasher;
        private Mock<IJwtProvider> jwtProvider;

        [SetUp]
        public void Setup()
        {
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mapper = UnitTestBusinessHelper.CreateMapperProfile();
            this.passwordHasher = new PasswordHasher();
            this.jwtProvider = new Mock<IJwtProvider>();
            this.userService = new UserService(mockUnitOfWork.Object, mapper, passwordHasher, jwtProvider.Object);
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
            var actualUsers = await userService.GetAllAsync(null);

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
            var createModel = new UserRegisterModel
            {
                Nickname = "NewUser",
                Email = "newuser@example.com",
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            mockUnitOfWork.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>()));
            // Act
            await userService.RegisterAsync(createModel);

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
            var invalidCreateModel = new UserRegisterModel { Nickname = string.Empty, Email = "" };

            // Act
            Func<Task> act = async () => await userService.RegisterAsync(invalidCreateModel);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task UpdateAsyncUpdatesUser()
        {
            // Arrange
            var userModel = new UserUpdateModel()
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
