using _2_Domain.Entities;

namespace _2_Domain.Interfaces;

public interface ILessonRepository
{
    Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId);
    Task<Lesson?> GetByIdAsync(Guid id);
    Task<bool> ExistsWithOrderAsync(Guid courseId, int order);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
}