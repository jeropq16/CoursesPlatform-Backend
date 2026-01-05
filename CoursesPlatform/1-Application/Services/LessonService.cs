using _1_Application.Interfaces;
using _2_Domain.Entities;
using _2_Domain.Interfaces;

namespace _1_Application.Services;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;

    public LessonService(
        ILessonRepository lessonRepository,
        ICourseRepository courseRepository)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Lesson?> CreateAsync(Guid courseId, string title, int order)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) return null;

        var exists = await _lessonRepository.ExistsWithOrderAsync(courseId, order);
        if (exists) return null;

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = title,
            Order = order,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _lessonRepository.AddAsync(lesson);
        return lesson;
    }

    public async Task<bool> SoftDeleteAsync(Guid lessonId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);
        if (lesson == null) return false;

        lesson.IsDeleted = true;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.UpdateAsync(lesson);
        return true;
    }

    public async Task<bool> ReorderAsync(Guid lessonId, int newOrder)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);
        if (lesson == null) return false;

        var exists = await _lessonRepository.ExistsWithOrderAsync(
            lesson.CourseId, newOrder);

        if (exists) return false;

        lesson.Order = newOrder;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.UpdateAsync(lesson);
        return true;
    }
}