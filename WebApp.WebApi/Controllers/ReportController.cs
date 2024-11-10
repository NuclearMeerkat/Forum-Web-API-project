using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.ReportModels;

namespace WebApp.WebApi.Controllers;

[Route("api")]
[ApiController]
public class ReportController : BaseController
{
    private readonly IReportService reportService;
    private readonly IServiceProvider serviceProvider;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ReportController(IReportService reportService, IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(reportService);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        this.reportService = reportService;
        this.serviceProvider = serviceProvider;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Retrieves all reports with optional query parameters for filtering, sordting and pagination.
    /// </summary>
    /// <param name="parametersModel">The query parameters for filtering and pagination of reports.</param>
    /// <returns>A list of reports that match the query parameters.</returns>
    [HttpGet("reports")]
    //[Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> GetAllReports([FromQuery] ReportQueryParametersModel parametersModel)
    {
        var validator = this.serviceProvider.GetService<IValidator<ReportQueryParametersModel>>();

        return await this.ValidateAndExecuteAsync(parametersModel, validator, async () =>
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
    //[Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> GetReportById(int userId, int messageId)
    {
        if (userId <= 0 || messageId <= 0)
        {
            return this.BadRequest("Invalid report ID.");
        }

        ReportSummaryModel report;
        try
        {
            report = await this.reportService.GetByIdAsync(userId, messageId);
        }
        catch (ForumException ex)
        {
            return this.NotFound(ex.Message);
        }

        return this.Ok(report);
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
        var validator = this.serviceProvider.GetService<IValidator<ReportCreateModel>>();

        model.UserId = GetCurrentUserId(httpContextAccessor);
        return await this.ValidateAndExecuteAsync(model, validator, async () =>
        {
            try
            {
                CompositeKey key = await this.reportService.AddAsync(model);
                return this.CreatedAtAction(nameof(this.GetReportById), model);
            }
            catch (ForumException e)
            {
                return this.NotFound(e.Message);
            }
            catch (DbUpdateException)
            {
                return this.Conflict("Report with this userid and messageid already exists.");
            }
        });
    }

    /// <summary>
    /// Updates the status of a specific report by message ID.
    /// </summary>
    /// <param name="messageId">The ID of the message associated with the report.</param>
    /// <param name="status">The new status for the report.</param>
    /// <returns>An Ok result with the updated report details, or a NotFound/BadRequest result if the update fails.</returns>
    [HttpPut("reports/{messageId:int}/status/{status:int}")]
    //[Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> UpdateReportStatus(int messageId, int status)
    {
        // Try to convert the int status parameter to the nullable enum type
        ReportStatus? reportStatus = Enum.IsDefined(typeof(ReportStatus), status)
            ? (ReportStatus?)status
            : null;

        if (reportStatus == null)
        {
            return this.BadRequest("Invalid status value provided.");
        }

        int userId = GetCurrentUserId(httpContextAccessor);

        var reportUpdateModel =
            new ReportUpdateModel() { UserId = userId, MessageId = messageId, Status = reportStatus };

        var validator = this.serviceProvider.GetService<IValidator<ReportUpdateModel>>();

        return await this.ValidateAndExecuteAsync(reportUpdateModel, validator, async () =>
        {
            try
            {
                await this.reportService.UpdateAsync(reportUpdateModel);
                var updatedMessage = await this.reportService.GetByIdAsync(userId, messageId);
                return this.Ok(updatedMessage);
            }
            catch (ForumException)
            {
                return this.NotFound($"Report with this Id was not found.");
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        });
    }

    /// <summary>
    /// Deletes a specific report by user ID and message ID.
    /// </summary>
    /// <param name="userId">The ID of the user who created the report.</param>
    /// <param name="messageId">The ID of the message associated with the report.</param>
    /// <returns>NoContent if deletion is successful, or a NotFound/BadRequest result if deletion fails.</returns>
    [HttpDelete("reports/{userId:int}/{messageId:int}")]
    //[Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> DeleteReport(int userId, int messageId)
    {
        var key = new CompositeKey() { KeyPart1 = userId, KeyPart2 = messageId };

        var validator = this.serviceProvider.GetService<IValidator<CompositeKey>>();

        return await this.ValidateAndExecuteAsync(key, validator, async () =>
        {
            if (key.KeyPart1 <= 0 || key.KeyPart2 <= 0)
            {
                return this.BadRequest("Invalid report ID.");
            }

            try
            {
                await this.reportService.DeleteAsync(key);
                return this.NoContent();
            }
            catch (ForumException ex)
            {
                return this.NotFound(ex.Message);
            }
        });
    }

    /// <summary>
    /// Retrieves all reports for a specific topic by topic ID.
    /// </summary>
    /// <param name="topicId">The ID of the topic for which reports are requested.</param>
    /// <returns>A list of reports associated with the specified topic, or a NotFound/BadRequest result if the topic is invalid.</returns>
    [HttpGet("topics/{topicId}/reports")]
    //[Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> GetReportsForTopic(int topicId)
    {
        if (topicId <= 0)
        {
            return this.BadRequest("Invalid topic ID.");
        }

        try
        {
            var reports = await this.reportService.GetReportsForTopicAsync(topicId);
            return this.Ok(reports);
        }
        catch (ForumException e)
        {
            return this.NotFound(e.Message);
        }
    }
}
