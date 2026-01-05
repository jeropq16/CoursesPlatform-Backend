namespace _1_Application.DTOs;

public class CourseSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int TotalLessons { get; set; }
    public DateTime LastModifiedAt { get; set; }
}