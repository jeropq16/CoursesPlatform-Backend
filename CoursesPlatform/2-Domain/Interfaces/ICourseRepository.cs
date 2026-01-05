using _2_Domain.Entities;
using _2_Domain.Enums;

namespace _2_Domain.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task<IEnumerable<Course>> SearchAsync(string? query, CourseStatus? status, int page, int pageSize);
    Task<int> CountAsync(string? query, CourseStatus? status);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
}