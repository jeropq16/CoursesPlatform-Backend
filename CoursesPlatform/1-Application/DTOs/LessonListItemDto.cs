namespace _1_Application.DTOs;

public class LessonListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
}