using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models;

namespace WebApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : ControllerBase
{
    private readonly ITopicService topicService;

    public TopicsController(ITopicService topicService)
    {
        this.topicService = topicService;
    }

    // GET: api/topics
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TopicQueryParametersModel parameters)
    {
        var topics = await this.topicService.GetTopicsAsync(parameters);
        return this.Ok(topics);
    }

    // GET: api/topics/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        TopicModel topic;
        try
        {
            topic = await this.topicService.GetByIdAsync(id);
        }
        catch (ForumException)
        {
            return this.NotFound();
        }

        return this.Ok(topic);
    }

    // POST: api/topics
    // [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TopicDtoModel topicDto)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(topicDto);
            await this.topicService.AddAsync(topicDto);
            return this.CreatedAtAction(nameof(this.GetById), new { id = topicDto.Id }, topicDto);
        }
        catch (ForumException e)
        {
            return this.BadRequest(e.Message);
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected exception occured in CreateTopic");
            throw;
        }
    }

    // PUT: api/topics/{id}
    // [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] TopicDtoModel topicDto)
    {
        try
        {
            topicDto.Id = id;
            await this.topicService.UpdateAsync(topicDto);
            return this.NoContent();
        }
        catch (ForumException)
        {
            return this.BadRequest();
        }
    }

    // DELETE: api/topics/{id}
    // [Authorize(Roles = "Admin,Moderator")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        await this.topicService.DeleteAsync(id);
        return this.NoContent();
    }
}
