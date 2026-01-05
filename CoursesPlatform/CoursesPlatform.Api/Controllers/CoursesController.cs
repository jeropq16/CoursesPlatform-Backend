using _1_Application.DTOs;
using _1_Application.Interfaces;
using _2_Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesPlatform.Api.Controllers;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesController  : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILessonService _lessonService;


    public CoursesController(ICourseService courseService, ILessonService lessonService)
    {
        _courseService = courseService;
        _lessonService = lessonService;

    }

    // POST /api/courses
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
    {
        var course = await _courseService.CreateAsync(request.Title);
        return Ok(course);
    }

    // PATCH /api/courses/{id}/publish
    [HttpPatch("{id}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        var result = await _courseService.PublishAsync(id);
        if (!result) return BadRequest("Course cannot be published");

        return NoContent();
    }

    // PATCH /api/courses/{id}/unpublish
    [HttpPatch("{id}/unpublish")]
    public async Task<IActionResult> Unpublish(Guid id)
    {
        var result = await _courseService.UnpublishAsync(id);
        if (!result) return NotFound();

        return NoContent();
    }

    // DELETE /api/courses/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _courseService.SoftDeleteAsync(id);
        if (!result) return NotFound();

        return NoContent();
    }

    // GET /api/courses/{id}/summary
    [HttpGet("{id}/summary")]
    public async Task<IActionResult> Summary(Guid id)
    {
        var summary = await _courseService.GetSummaryAsync(id);
        if (summary == null) return NotFound();

        return Ok(summary);
    }

    // GET /api/courses/search
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] CourseStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(new
        {
            Data = await _courseService.SearchAsync(q, status, page, pageSize),
            Page = page,
            PageSize = pageSize
        });
    }
    
    [HttpGet("{id}/lessons")]
    public async Task<IActionResult> GetLessons(Guid id)
    {
        var lessons = await _lessonService.GetByCourseAsync(id);
        return Ok(lessons);
    }
    
    [HttpPost("import")]
    [Authorize]
    public async Task<IActionResult> ImportFromExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required");

        if (!file.FileName.EndsWith(".xlsx"))
            return BadRequest("Only .xlsx files are allowed");

        using var stream = file.OpenReadStream();
        var result = await _courseService.ImportFromExcelAsync(stream);

        return Ok(result);
    }

}