namespace _1_Application.DTOs;

public class CreateLessonRequest
{
    public Guid CourseId { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
}