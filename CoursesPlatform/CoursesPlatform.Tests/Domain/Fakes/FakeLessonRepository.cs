using _2_Domain.Entities;
using _2_Domain.Interfaces;

namespace CoursesPlatform.Tests.Domain.Fakes;

public class FakeLessonRepository : ILessonRepository
{
    private readonly List<Lesson> _lessons = new();

    public Task AddAsync(Lesson lesson)
    {
        _lessons.Add(lesson);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Lesson lesson)
    {
        return Task.CompletedTask;
    }

    public Task<Lesson?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_lessons.FirstOrDefault(l => l.Id == id));
    }

    public Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId)
    {
        return Task.FromResult(_lessons.Where(l => l.CourseId == courseId));
    }

    public Task<bool> ExistsWithOrderAsync(Guid courseId, int order)
    {
        return Task.FromResult(
            _lessons.Any(l => l.CourseId == courseId && l.Order == order)
        );
    }
}