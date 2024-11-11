using AutoMapper;
using FluentAssertions;
using Moq;
using WebApp.BusinessLogic.Services;
using WebApp.BusinessLogic.Validation;
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
        this.userService = new UserService(this.mockUnitOfWork.Object, this.mapper, this.passwordHasher, this.jwtProvider.Object);
    }

    [Test]
    public async Task GetAllAsyncReturnsAllUsers()
    {
        // Arrange
        var expectedUsers = this.GetTestUserModels();
        this.mockUnitOfWork
            .Setup(u => u.UserRepository.GetAllAsync())
            .ReturnsAsync(this.GetTestUserEntities());

        // Act
        var actualUsers = await this.userService.GetAllAsync(null);

        // Assert
        actualUsers.Should().BeEquivalentTo(expectedUsers);
    }

    [Test]
    public async Task GetByIdAsyncReturnsCorrectUserModel()
    {
        // Arrange
        var expectedUser = this.GetTestUserModels().First();
        int userId = expectedUser.Id;
        this.mockUnitOfWork
            .Setup(u => u.UserRepository.GetByIdAsync(It.IsAny<object[]>()))
            .ReturnsAsync(this.GetTestUserEntities().First());

        // Act
        var actualUser = await this.userService.GetByIdAsync(userId);

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

        this.mockUnitOfWork.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>()));
        // Act
        await this.userService.RegisterAsync(createModel);

        // Assert
        this.mockUnitOfWork.Verify(u => u.UserRepository.AddAsync(It.Is<User>(
            user => user.Nickname == createModel.Nickname &&
                    user.Email == createModel.Email &&
                    user.Role == createModel.Role &&
                    user.CreatedAt == createModel.CreatedAt)), Times.Once);
        this.mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task AddAsyncThrowsExceptionWhenCreateModelIsInvalid()
    {
        // Arrange
        var invalidCreateModel = new UserRegisterModel { Nickname = string.Empty, Email = "" };

        // Act
        Func<Task> act = async () => await this.userService.RegisterAsync(invalidCreateModel);

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

        this.mockUnitOfWork.Setup(u => u.UserRepository.Update(It.IsAny<User>()));

        // Act
        await this.userService.UpdateAsync(userModel);

        // Assert
        this.mockUnitOfWork.Verify(u => u.UserRepository.Update(It.Is<User>(
            user =>
                    user.Nickname == userModel.Nickname &&
                    user.Email == userModel.Email)), Times.Once);
        this.mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task DeleteAsyncDeletesUser()
    {
        // Arrange
        int userId = 1;
        this.mockUnitOfWork.Setup(u => u.UserRepository.DeleteByIdAsync(It.IsAny<int>()));

        // Act
        await this.userService.DeleteAsync(userId);

        // Assert
        this.mockUnitOfWork.Verify(u => u.UserRepository.DeleteByIdAsync(userId), Times.Once);
        this.mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
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
