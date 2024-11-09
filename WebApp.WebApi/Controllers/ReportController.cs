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

    public ReportController(IReportService reportService, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(reportService);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        this.reportService = reportService;
        this.serviceProvider = serviceProvider;
    }

    // GET: api/reports
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

    // GET: api/reports/{id}
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

    // POST: api/reports
    [HttpPost("reports")]
    //[Authorize]
    public async Task<IActionResult> SubmitReport([FromBody] ReportCreateModel model)
    {
        var validator = this.serviceProvider.GetService<IValidator<ReportCreateModel>>();

        return await this.ValidateAndExecuteAsync(model, validator, async () =>
        {
            try
            {
                CompositeKey key = await this.reportService.RegisterAsync(model);
                return this.CreatedAtAction(nameof(this.GetReportById), model);
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (DbUpdateException)
            {
                return this.Conflict("Report with this userid and messageid already exists.");
            }
        });
    }

    // PUT: api/reports/{id}/status
    [HttpPut("reports/{userId:int}/{messageId:int}/status/{status:int}")]
    //[Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> UpdateReportStatus(int userId, int messageId, int status)
    {
        // Try to convert the int status parameter to the nullable enum type
        ReportStatus? reportStatus = Enum.IsDefined(typeof(ReportStatus), status)
            ? (ReportStatus?)status
            : null;

        if (reportStatus == null)
        {
            return this.BadRequest("Invalid status value provided.");
        }

        var reportUpdateModel = new ReportUpdateModel() { UserId = userId, MessageId = messageId, Status = reportStatus };

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

    // DELETE: api/reports/{id}
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

    // GET: api/topics/{topicId}/reports
    [HttpGet("topics/{topicId}/reports")]
    //[Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> GetReportsForTopic(int topicId)
    {
        if (topicId <= 0)
        {
            return this.BadRequest("Invalid topic ID.");
        }

        var reports = await this.reportService.GetReportsForTopicAsync(topicId);
        return this.Ok(reports);
    }
}
