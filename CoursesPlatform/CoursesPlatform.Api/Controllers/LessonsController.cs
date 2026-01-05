using _1_Application.DTOs;
using _1_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesPlatform.Api.Controllers;

[ApiController]
[Route("api/lessons")]
[Authorize]
public class LessonsController  : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    // POST /api/lessons
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLessonRequest request)
    {
        var lesson = await _lessonService.CreateAsync(
            request.CourseId,
            request.Title,
            request.Order);

        if (lesson == null) return BadRequest("Invalid lesson data");

        return Ok(new LessonResponseDto
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Title = lesson.Title,
            Order = lesson.Order
        });

    }

    // DELETE /api/lessons/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _lessonService.SoftDeleteAsync(id);
        if (!result) return NotFound();

        return NoContent();
    }

    // PATCH /api/lessons/{id}/reorder
    [HttpPatch("{id}/reorder")]
    public async Task<IActionResult> Reorder(Guid id, [FromBody] ReorderLessonRequest request)
    {
        var result = await _lessonService.ReorderAsync(id, request.NewOrder);
        if (!result) return BadRequest("Invalid order");

        return NoContent();
    }
}