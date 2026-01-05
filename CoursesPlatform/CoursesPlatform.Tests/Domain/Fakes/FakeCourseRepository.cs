using _2_Domain.Entities;
using _2_Domain.Enums;
using _2_Domain.Interfaces;

namespace CoursesPlatform.Tests.Domain.Fakes;

public class FakeCourseRepository : ICourseRepository
{
    private readonly List<Course> _courses = new();

    public Task AddAsync(Course course)
    {
        _courses.Add(course);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Course course)
    {
        return Task.CompletedTask;
    }

    public Task<Course?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_courses.FirstOrDefault(c => c.Id == id));
    }

    public Task<IEnumerable<Course>> SearchAsync(
        string? query,
        CourseStatus? status,
        int page,
        int pageSize)
    {
        var data = _courses.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
            data = data.Where(c => c.Title.Contains(query));

        if (status.HasValue)
            data = data.Where(c => c.Status == status);

        return Task.FromResult(data);
    }

    public Task<int> CountAsync(string? query, CourseStatus? status)
    {
        return Task.FromResult(_courses.Count);
    }
}