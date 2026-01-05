using _2_Domain.Enums;

namespace _2_Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public CourseStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}