using WebApp.Core.Entities;
using WebApp.Core.Enums;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repositories;
using WebApp.Tests.Comparers;

namespace WebApp.Tests.Data;
internal class ReportRepositoryTests
{
    [TestCase(1, 1)]
    [TestCase(2, 1)]
    public async Task ReportRepositoryGetByIdAsyncReturnsSingleValue(int UserId, int MessageId)
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var reportRepository = new ReportRepository(context);

        var report = await reportRepository.GetByIdAsync(UserId, MessageId);

        var expected = ExpectedReports.FirstOrDefault(x => x.MessageId == MessageId && x.UserId == UserId);

        Assert.That(report, Is.EqualTo(expected).Using(new ReportEqualityComparer()), message: "GetByIdAsync method works incorrect");
    }

    [Test]
    public async Task ReportRepositoryGetAllAsyncReturnsAllValues()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var reportRepository = new ReportRepository(context);

        var reports = await reportRepository.GetAllAsync();

        Assert.That(reports, Is.EqualTo(ExpectedReports).Using(new ReportEqualityComparer()), message: "GetAllAsync method works incorrect");
    }

    [Test]
    public async Task ReportRepositoryAddAsyncAddsValueToDatabase()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var reportRepository = new ReportRepository(context);
        var report = new Report { UserId = 1, MessageId = 1, Reason = "Off-topic", Status = ReportStatus.Pending, CreatedAt = DateTime.UtcNow };

        await reportRepository.AddAsync(report);
        await context.SaveChangesAsync();

        Assert.That(context.Reports.Count(), Is.EqualTo(3), message: "AddAsync method works incorrect");
    }

    [Test]
    public async Task ReportRepositoryDeleteByIdAsyncDeletesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var reportRepository = new ReportRepository(context);

        await reportRepository.DeleteByIdAsync(3, 2); // 1.UserId, MessageId
        await context.SaveChangesAsync();

        Assert.That(context.Reports.Count(), Is.EqualTo(1), message: "DeleteByIdAsync works incorrect");
    }

    [Test]
    public async Task ReportRepositoryUpdateUpdatesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var reportRepository = new ReportRepository(context);
        var report = new Report
        {
            UserId = 2,
            MessageId = 1,
            Reason = "Updated Reason",
            Status = ReportStatus.Resolved,
            CreatedAt = DateTime.UtcNow
        };

        reportRepository.Update(report);
        await context.SaveChangesAsync();

        Assert.That(report, Is.EqualTo(new Report
        {
            UserId = 2,
            MessageId = 1,
            Reason = "Updated Reason",
            Status = ReportStatus.Resolved,
            CreatedAt = DateTime.UtcNow
        }).Using(new ReportEqualityComparer()), message: "Update method works incorrect");
    }

    private static IEnumerable<Report> ExpectedReports =>
        new[]
        {
            new Report { UserId = 3, MessageId = 2, Reason = "Spam content", Status = ReportStatus.Pending },
            new Report { UserId = 2, MessageId = 1, Reason = "Inappropriate language", Status = ReportStatus.Resolved }
        };
}
