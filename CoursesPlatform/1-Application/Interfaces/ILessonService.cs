using _2_Domain.Entities;

namespace _1_Application.Interfaces;

public interface ILessonService
{
    Task<Lesson?> CreateAsync(Guid courseId, string title, int order);
    Task<bool> SoftDeleteAsync(Guid lessonId);
    Task<bool> ReorderAsync(Guid lessonId, int newOrder);
}