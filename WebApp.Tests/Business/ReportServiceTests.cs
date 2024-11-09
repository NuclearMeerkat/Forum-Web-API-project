using AutoMapper;
using FluentAssertions;
using Moq;
using WebApp.BusinessLogic.Services;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Models.ReportModels;

namespace WebApp.Tests.Business;

public class ReportServiceTests
{
private Mock<IUnitOfWork> mockUnitOfWork;
        private IMapper mapper;
        private ReportService reportService;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mapper = UnitTestBusinessHelper.CreateMapperProfile();
            reportService = new ReportService(mockUnitOfWork.Object, mapper);
        }

        [Test]
        public async Task GetAllAsyncReturnsAllReports()
        {
            // Arrange
            var expected = GetTestReportModels();
            mockUnitOfWork
                .Setup(u => u.ReportRepository.GetAllAsync())
                .ReturnsAsync(GetTestReportEntities());

            // Act
            var actual = await reportService.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsyncReturnsCorrectReportModel()
        {
            // Arrange
            var expected = GetTestReportModels().First();
            int reportUserId = expected.UserId;
            int reportMessageId = expected.MessageId;
            mockUnitOfWork
                .Setup(u => u.ReportRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GetTestReportEntities().First());

            // Act
            var actual = await reportService.GetByIdAsync(reportUserId, reportMessageId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task AddAsyncAddsReport()
        {
            // Arrange
            var createModel = new ReportCreateModel { UserId = 1, MessageId = 1, Reason = "Test reason" };
            mockUnitOfWork.Setup(u => u.ReportRepository.AddAsync(It.IsAny<Report>()));

            // Act
            await reportService.RegisterAsync(createModel);

            // Assert
            mockUnitOfWork.Verify(u => u.ReportRepository.AddAsync(It.Is<Report>(
                r => r.UserId == createModel.UserId && r.MessageId == createModel.MessageId && r.Reason == createModel.Reason)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task AddAsyncThrowsForumExceptionWhenCreateModelIsInvalid()
        {
            // Arrange
            var invalidCreateModel = new ReportCreateModel { UserId = 0, MessageId = 0, Reason = string.Empty };

            // Act
            Func<Task> act = async () => await reportService.RegisterAsync(invalidCreateModel);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task UpdateAsyncUpdatesReport()
        {
            // Arrange
            var reportModel = new ReportUpdateModel() { UserId = 1, MessageId = 1, Reason = "Updated reason" };
            mockUnitOfWork.Setup(u => u.ReportRepository.Update(It.IsAny<Report>()));

            // Act
            await reportService.UpdateAsync(reportModel);

            // Assert
            mockUnitOfWork.Verify(u => u.ReportRepository.Update(It.Is<Report>(
                r => r.UserId == reportModel.UserId && r.MessageId == reportModel.MessageId && r.Reason == reportModel.Reason)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsyncThrowsForumExceptionWhenModelIsInvalid()
        {
            // Arrange
            var invalidModel = new ReportUpdateModel() { UserId = 0, MessageId = 0, Reason = string.Empty };

            // Act
            Func<Task> act = async () => await reportService.UpdateAsync(invalidModel);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task DeleteAsyncDeletesReport()
        {
            // Arrange
            var key = new CompositeKey() { KeyPart1 = 1, KeyPart2 = 1 };
            mockUnitOfWork.Setup(u => u.ReportRepository.DeleteByIdAsync(It.IsAny<int>()));

            // Act
            await reportService.DeleteAsync(key);

            // Assert
            mockUnitOfWork.Verify(u => u.ReportRepository.DeleteByIdAsync(key), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        // Helper methods for test data
        private List<ReportModel> GetTestReportModels()
        {
            return new List<ReportModel>
            {
                new ReportModel
                {
                    UserId = 1,
                    MessageId = 1,
                    Reason = "Reason 1",
                    Status = Infrastructure.Enums.ReportStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                },
                new ReportModel
                {
                    UserId = 2,
                    MessageId = 2,
                    Reason = "Reason 2",
                    Status = Infrastructure.Enums.ReportStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }

        private List<Report> GetTestReportEntities()
        {
            return new List<Report>
            {
                new Report
                {
                    UserId = 1,
                    MessageId = 1,
                    Reason = "Reason 1",
                    Status = Infrastructure.Enums.ReportStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                },
                new Report
                {
                    UserId = 2,
                    MessageId = 2,
                    Reason = "Reason 2",
                    Status = Infrastructure.Enums.ReportStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
}
