using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.ReportModels;
using WebApp.WebApi.Utilities;

namespace WebApp.WebApi.Controllers;

[Route("api")]
[ApiController]
public class ReportController : BaseController
{
    private readonly IReportService reportService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly RequestProcessor requestProcessor;

    public ReportController(
        IReportService reportService,
        IHttpContextAccessor httpContextAccessor,
        RequestProcessor requestProcessor)
    {
        ArgumentNullException.ThrowIfNull(reportService);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(requestProcessor);
        this.reportService = reportService;
        this.httpContextAccessor = httpContextAccessor;
        this.requestProcessor = requestProcessor;
    }

    /// <summary>
    /// Retrieves all reports with optional query parameters for filtering, sordting and pagination.
    /// </summary>
    /// <param name="parametersModel">The query parameters for filtering and pagination of reports.</param>
    /// <returns>A list of reports that match the query parameters.</returns>
    [HttpGet("reports")]
    [Authorize(Policy = "ModeratorAccess")]
    public async Task<IActionResult> GetAllReports([FromQuery] ReportQueryParametersModel parametersModel)
    {
        return await this.requestProcessor.ProcessRequestAsync(parametersModel, async () =>
        {
            var reports = await this.reportService.GetAllAsync(parametersModel);
            return this.Ok(reports);
        });
    }

    /// <summary>
    /// Retrieves a specific report by user ID and message ID.
    /// </summary>
    /// <param name="userId">The ID of the user who created the report.</param>
    /// <param name="messageId">The ID of the message associated with the report.</param>
    /// <returns>The report if found, otherwise a NotFound or BadRequest result.</returns>
    [HttpGet("reports/{userId:int}/{messageId:int}")]
    [Authorize(Policy = "ModeratorAccess")]
    public async Task<IActionResult> GetReportById(int userId, int messageId)
    {
        ReportCompositeKey key = new ReportCompositeKey() { UserId = userId, MessageId = messageId };
        return await this.requestProcessor.ProcessRequestAsync(key, async () =>
        {
            var report = await this.reportService.GetByIdAsync(key.UserId, key.MessageId);
            return this.Ok(report);
        });
    }

    /// <summary>
    /// Submits a new report.
    /// </summary>
    /// <param name="model">The model containing report details to be submitted.</param>
    /// <returns>A CreatedAtAction result with the created report's details, or a BadRequest/Conflict result if submission fails.</returns>
    [HttpPost("reports")]
    [Authorize]
    public async Task<IActionResult> SubmitReport([FromBody] ReportCreateModel model)
    {
        model.UserId = GetCurrentUserId(this.httpContextAccessor);
        return await this.requestProcessor.ProcessRequestAsync(model, async () =>
        {
            _ = await this.reportService.AddAsync(model);
            return this.CreatedAtAction(nameof(this.SubmitReport), model);
        });
    }

    /// <summary>
    /// Updates the status of a specific report by message ID.
    /// </summary>
    /// <returns>An Ok result with the updated report details, or a NotFound/BadRequest result if the update fails.</returns>
    [HttpPatch("reports/status/")]
    [Authorize(Policy = "ModeratorAccess")]
    public async Task<IActionResult> UpdateReportStatus(ReportStatusUpdateModel model)
    {
        return await this.requestProcessor.ProcessRequestAsync(model, async () =>
        {
            int userId = GetCurrentUserId(this.httpContextAccessor);
            var reportUpdateModel = new ReportUpdateModel()
            {
                UserId = userId,
                MessageId = model.MessageId,
                Status = model.Status,
            };
            await this.reportService.UpdateAsync(reportUpdateModel);
            var updatedMessage = await this.reportService.GetByIdAsync(userId, model.MessageId);
            return this.Ok(updatedMessage);
        });
    }

    /// <summary>
    /// Deletes a specific report by user ID and message ID.
    /// </summary>
    /// <param name="userId">The ID of the user who created the report.</param>
    /// <param name="messageId">The ID of the message associated with the report.</param>
    /// <returns>NoContent if deletion is successful, or a NotFound/BadRequest result if deletion fails.</returns>
    [HttpDelete("reports/{userId:int}/{messageId:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteReport(int userId, int messageId)
    {
        ReportCompositeKey key = new ReportCompositeKey() { UserId = userId, MessageId = messageId };
        return await this.requestProcessor.ProcessRequestAsync(key, async () =>
        {
            await this.reportService.DeleteAsync(key);
            return this.NoContent();
        });
    }

    /// <summary>
    /// Retrieves all reports for a specific topic by topic ID.
    /// </summary>
    /// <param name="topicId">The ID of the topic for which reports are requested.</param>
    /// <returns>A list of reports associated with the specified topic, or a NotFound/BadRequest result if the topic is invalid.</returns>
    [HttpGet("topics/{topicId}/reports")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetReportsForTopic(int topicId)
    {
        return await this.requestProcessor.ProcessRequestAsync(topicId, async () =>
        {
            var reports = await this.reportService.GetReportsForTopicAsync(topicId);
            return this.Ok(reports);
        });
    }
}
