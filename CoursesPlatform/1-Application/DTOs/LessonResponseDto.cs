namespace _1_Application.DTOs;

public class LessonResponseDto
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
}