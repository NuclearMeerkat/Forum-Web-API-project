using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models.ReportModels;

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
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> GetAllReports([FromQuery] ReportQueryParameters parameters)
    {
        var validator = this.serviceProvider.GetService<IValidator<ReportQueryParameters>>();

        return await this.ValidateAndExecuteAsync(parameters, validator, async () =>
        {
            var reports = await this.reportService.GetAllReportsAsync(parameters);
            return this.Ok(reports);
        });
    }

    // GET: api/reports/{id}
    [HttpGet("reports/{id:int}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> GetReportById(int id)
    {
        if (id <= 0)
        {
            return this.BadRequest("Invalid report ID.");
        }

        ReportModel report;
        try
        {
            report = await this.reportService.GetByIdAsync(id);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound($"Report with ID {id} not found.");
        }

        return this.Ok(report);
    }

    // POST: api/reports
    [HttpPost("reports")]
    [Authorize]
    public async Task<IActionResult> SubmitReport([FromBody] ReportCreateModel model)
    {
        var validator = this.serviceProvider.GetService<IValidator<ReportCreateModel>>();

        return await this.ValidateAndExecuteAsync(model, validator, async () =>
        {
            try
            {
                CompositeKey key = await this.reportService.AddAsync(model);
                return this.CreatedAtAction(nameof(this.GetReportById), model);
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
        });
    }

    // PUT: api/reports/{id}/status
    [HttpPut("reports/{id:int}/status")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> UpdateReportStatus(int id, [FromBody] ReportStatusUpdateModel model)
    {
        model.ReportId = id;

        var validator = this.serviceProvider.GetService<IValidator<ReportStatusUpdateModel>>();

        return await this.ValidateAndExecuteAsync(model, validator, async () =>
        {
            try
            {
                await this.reportService.UpdateReportStatusAsync(id, model);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Report with ID {id} not found.");
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        });
    }

    // DELETE: api/reports/{id}
    [HttpDelete("reports/{id:int}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> DeleteReport(int id)
    {
        if (id <= 0)
        {
            return this.BadRequest("Invalid report ID.");
        }

        try
        {
            await this.reportService.DeleteAsync(id);
            return this.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return this.NotFound($"Report with ID {id} not found.");
        }
    }

    // GET: api/topics/{topicId}/reports
    [HttpGet("topics/{topicId}/reports")]
    [Authorize(Roles = "Admin,Moderator")]
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
