using Moq;
using FluentAssertions;
using AutoMapper;
using WebApp.BusinessLogic.Services;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Models.MessageModels;
using WebApp.Infrastructure.Models.ReportModels;

namespace WebApp.Tests.Business
{
    public class MessageServiceTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private IMapper mapper;
        private MessageService messageService;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mapper = UnitTestBusinessHelper.CreateMapperProfile();
            messageService = new MessageService(mockUnitOfWork.Object, mapper);
        }

        [Test]
        public async Task GetAllAsyncReturnsAllMessages()
        {
            // Arrange
            var expected = GetTestMessageModels();
            mockUnitOfWork
                .Setup(u => u.MessageRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(GetTestMessageEntities());

            // Act
            var actual = await messageService.GetAllAsync(null);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsyncReturnsCorrectMessageModel()
        {
            // Arrange
            var expected = GetTestMessageModels().First();
            var messageId = expected.Id;
            mockUnitOfWork
                .Setup(u => u.MessageRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestMessageEntities().First());

            // Act
            var actual = await messageService.GetByIdAsync(messageId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task AddAsyncAddsMessage()
        {
            // Arrange
            var createModel = new MessageCreateModel { Content = "New Message", UserId = 1, TopicId = 2 };
            mockUnitOfWork.Setup(u => u.MessageRepository.AddAsync(It.IsAny<Message>()));

            // Act
            await messageService.RegisterAsync(createModel);

            // Assert
            mockUnitOfWork.Verify(u => u.MessageRepository.AddAsync(It.Is<Message>(
                m => m.Content == createModel.Content && m.UserId == createModel.UserId)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task AddAsyncThrowsForumExceptionWhenCreateModelIsInvalid()
        {
            // Arrange
            var invalidCreateModel = new MessageCreateModel { Content = string.Empty, UserId = 1 };

            // Act
            Func<Task> act = async () => await messageService.RegisterAsync(invalidCreateModel);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task DeleteAsyncDeletesMessage()
        {
            // Arrange
            var messageId = 1;
            mockUnitOfWork.Setup(u => u.MessageRepository.DeleteByIdAsync(It.IsAny<int>()));

            // Act
            await messageService.DeleteAsync(messageId);

            // Assert
            mockUnitOfWork.Verify(u => u.MessageRepository.DeleteByIdAsync(messageId), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task LikeMessageAddsLike()
        {
            // Arrange
            var userId = 1;
            var messageId = 2;
            mockUnitOfWork.Setup(u => u.MessageLikeRepository.AddAsync(It.IsAny<MessageLike>()));

            // Act
            await messageService.LikeMessage(userId, messageId);

            // Assert
            mockUnitOfWork.Verify(u => u.MessageLikeRepository.AddAsync(It.Is<MessageLike>(
                ml => ml.UserId == userId && ml.MessageId == messageId)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveLikeRemovesLike()
        {
            // Arrange
            var userId = 1;
            var messageId = 2;
            mockUnitOfWork.Setup(u => u.MessageLikeRepository.DeleteByIdAsync(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            await messageService.RemoveLike(userId, messageId);

            // Assert
            mockUnitOfWork.Verify(u => u.MessageLikeRepository.DeleteByIdAsync(userId, messageId), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        // Helper methods for test data
        private List<MessageModel> GetTestMessageModels()
        {
            return new List<MessageModel>
    {
        new MessageModel
        {
            Id = 1,
            UserId = 1,
            TopicId = 1,
            Content = "Test Message 1",
            CreatedAt = DateTime.Today,
            IsEdited = false,
            LikesCounter = 5,
            ParentMessageId = null,
            Replies = new List<MessageModel>
            {
                new MessageModel
                {
                    Id = 2,
                    UserId = 2,
                    TopicId = 1,
                    Content = "Reply to Test Message 1",
                    CreatedAt = DateTime.Today,
                    IsEdited = true,
                    LikesCounter = 2,
                    ParentMessageId = 1,
                    Reports = new List<ReportModel>
                    {
                        new ReportModel { UserId = 1, MessageId = 2, Reason = "Inappropriate content", CreatedAt = DateTime.Today }
                    }
                },
                new MessageModel
                {
                    Id = 3,
                    UserId = 3,
                    TopicId = 1,
                    Content = "Another reply to Test Message 1",
                    CreatedAt = DateTime.Today,
                    IsEdited = false,
                    LikesCounter = 1,
                    ParentMessageId = 1
                }
            },
            Reports = new List<ReportModel>
            {
                new ReportModel { UserId = 1, MessageId = 1, Reason = "Spam", CreatedAt = DateTime.Today }
            }
        },
        new MessageModel
        {
            Id = 4,
            UserId = 4,
            TopicId = 2,
            Content = "Test Message 2 (no replies)",
            CreatedAt = DateTime.Today,
            IsEdited = false,
            LikesCounter = 10,
            ParentMessageId = null,
            Replies = new List<MessageModel>(),
            Reports = new List<ReportModel>()
        },
        new MessageModel
        {
            Id = 5,
            UserId = 5,
            TopicId = 3,
            Content = "Test Message 3 (no replies)",
            CreatedAt = DateTime.Today,
            IsEdited = false,
            LikesCounter = 3,
            ParentMessageId = null,
            Replies = new List<MessageModel>(),
            Reports = new List<ReportModel>()
        }
    };
        }

        private List<Message> GetTestMessageEntities()
        {
            return new List<Message>
    {
        new Message
        {
            Id = 1,
            UserId = 1,
            TopicId = 1,
            Content = "Test Message 1",
            CreatedAt = DateTime.Today,
            IsEdited = false,
            LikesCounter = 5,
            ParentMessageId = null,
            Replies = new List<Message>
            {
                new Message
                {
                    Id = 2,
                    UserId = 2,
                    TopicId = 1,
                    Content = "Reply to Test Message 1",
                    CreatedAt = DateTime.Today,
                    IsEdited = true,
                    LikesCounter = 2,
                    ParentMessageId = 1,
                    Reports = new List<Report>
                    {
                        new Report { UserId = 1, MessageId = 2, Reason = "Inappropriate content", CreatedAt = DateTime.Today }
                    }
                },
                new Message
                {
                    Id = 3,
                    UserId = 3,
                    TopicId = 1,
                    Content = "Another reply to Test Message 1",
                    CreatedAt = DateTime.Today,
                    IsEdited = false,
                    LikesCounter = 1,
                    ParentMessageId = 1
                }
            },
            Reports = new List<Report>
            {
                new Report { UserId = 1, MessageId = 1, Reason = "Spam", CreatedAt = DateTime.Today }
            }
        },
        new Message
        {
            Id = 4,
            UserId = 4,
            TopicId = 2,
            Content = "Test Message 2 (no replies)",
            CreatedAt = DateTime.Today,
            IsEdited = false,
            LikesCounter = 10,
            ParentMessageId = null,
            Replies = new List<Message>(),
            Reports = new List<Report>()
        },
        new Message
        {
            Id = 5,
            UserId = 5,
            TopicId = 3,
            Content = "Test Message 3 (no replies)",
            CreatedAt = DateTime.Today,
            IsEdited = false,
            LikesCounter = 3,
            ParentMessageId = null,
            Replies = new List<Message>(),
            Reports = new List<Report>()
        }
    };
        }
    }
}
