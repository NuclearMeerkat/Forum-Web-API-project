using AutoMapper;
using FluentAssertions;
using Moq;
using WebApp.BusinessLogic.Services;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Models;
using WebApp.DataAccess.Repositories;

namespace WebApp.Tests.Business;

public class TopicServiceTests
{
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<ITopicRepository> mockTopicRepository;
        private IMapper mapper;
        private TopicService topicService;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mapper = UnitTestBusinessHelper.CreateMapperProfile();
            topicService = new TopicService(mockUnitOfWork.Object, mapper);
        }

        [Test]
        public async Task AddAsyncAddsTopic()
        {
            // Arrange
            var createModel = new TopicDtoModel { UserId = 1, Title = "New Topic", Description = "New topic description" };
            mockUnitOfWork.Setup(u => u.TopicRepository.AddAsync(It.IsAny<Topic>()));

            // Act
            await topicService.AddAsync(createModel);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository.AddAsync(It.Is<Topic>(
                t => t.UserId == createModel.UserId && t.Title == createModel.Title && t.Description == createModel.Description)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task AddAsyncThrowsExceptionWhenCreateModelIsInvalid()
        {
            // Arrange
            var invalidCreateModel = new TopicDtoModel { UserId = 0, Title = string.Empty };

            // Act
            Func<Task> act = async () => await topicService.AddAsync(invalidCreateModel);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task UpdateAsyncUpdatesTopic()
        {
            // Arrange
            var testTopicModel = new TopicDtoModel()
            {
                Id = 1,
                UserId = 1,
                Title = "Updated Topic",
                Description = "Updated description",
                CreatedAt = DateTime.Today
            };

            var testTopicEntity = new Topic
            {
                Id = testTopicModel.Id,
                UserId = testTopicModel.UserId,
                Title = testTopicModel.Title,
                Description = testTopicModel.Description,
                CreatedAt = testTopicModel.CreatedAt
            };

            mockTopicRepository = new Mock<ITopicRepository>();
            mockUnitOfWork.Setup(u => u.TopicRepository).Returns(mockTopicRepository.Object);
            mockTopicRepository.Setup(r => r.Update(It.IsAny<Topic>()));

            // Act
            await topicService.UpdateAsync(testTopicModel);

            // Assert
            mockTopicRepository.Verify(r => r.Update(It.Is<Topic>(t =>
                t.Id == testTopicModel.Id &&
                t.UserId == testTopicModel.UserId &&
                t.Title == testTopicModel.Title &&
                t.Description == testTopicModel.Description)), Times.Once);

            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsyncDeletesTopic()
        {
            // Arrange
            int topicId = 1;
            mockUnitOfWork.Setup(u => u.TopicRepository.DeleteByIdAsync(It.IsAny<int>()));

            // Act
            await topicService.DeleteAsync(topicId);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository.DeleteByIdAsync(topicId), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        // Helper methods for test data
        private List<TopicModel> GetTestTopicModels()
        {
            return new List<TopicModel>
            {
                new TopicModel
                {
                    Id = 1,
                    UserId = 1,
                    Title = "Test Topic 1",
                    Description = "Description of Test Topic 1",
                    CreatedAt = DateTime.Today,
                    AverageStars = 4.5, // Example average
                    EvaluationsNumber = 2, // Example count
                    ActivityScore = 75.0,
                    CreatorNickname = "User1",
                    CreatorEmail = "user1@example.com",
                    CreatorProfilePictureUrl = null,
                    Messages = new List<MessageModel>
                    {
                        new MessageModel { Id = 1, UserId = 1, TopicId = 1, Content = "Message 1", CreatedAt = DateTime.Today }
                    }
                },
                new TopicModel
                {
                    Id = 2,
                    UserId = 2,
                    Title = "Test Topic 2",
                    Description = "Description of Test Topic 2",
                    CreatedAt = DateTime.Today,
                    AverageStars = 4, // No stars, so average is zero
                    EvaluationsNumber = 1, // No stars, so count is zero
                    ActivityScore = 50.0,
                    CreatorNickname = "User2",
                    CreatorEmail = "user2@example.com",
                    CreatorProfilePictureUrl = "https://example.com/user2.jpg",
                    Messages = new List<MessageModel>()
                }
            };
        }

        private List<Topic> GetTestTopicEntities()
        {
            return new List<Topic>
            {
                new Topic
                {
                    Id = 1,
                    UserId = 1,
                    Title = "Test Topic 1",
                    Description = "Description of Test Topic 1",
                    CreatedAt = DateTime.Today,
                    User = new User { Id = 1, Nickname = "User1", Email = "user1@example.com" },
                    Messages = new List<Message>
                    {
                        new Message { Id = 1, UserId = 1, TopicId = 1, Content = "Message 1", CreatedAt = DateTime.Today }
                    },
                    Stars = new List<TopicStars>
                    {
                        new TopicStars { StarCount = 5, UserId = 1, TopicId = 1 },
                        new TopicStars { StarCount = 4, UserId = 2, TopicId = 1 }
                    }
                },
                new Topic
                {
                    Id = 2,
                    UserId = 2,
                    Title = "Test Topic 2",
                    Description = "Description of Test Topic 2",
                    CreatedAt = DateTime.Today,
                    User = new User { Id = 2, Nickname = "User2", Email = "user2@example.com", ProfilePictureUrl = "https://example.com/user2.jpg" },
                    Messages = new List<Message>(),
                    Stars = new List<TopicStars>
                    {
                        new TopicStars { StarCount = 4, UserId = 3, TopicId = 2 }
                    }
                }
            };
        }
}
