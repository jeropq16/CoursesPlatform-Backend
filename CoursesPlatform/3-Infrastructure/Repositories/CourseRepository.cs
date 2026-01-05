using _2_Domain.Entities;
using _2_Domain.Enums;
using _2_Domain.Interfaces;
using _3_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace _3_Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Course>> SearchAsync(string? query, CourseStatus? status, int page, int pageSize)
    {
        var courses = _context.Courses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            courses = courses.Where(c => c.Title.Contains(query));

        if (status.HasValue)
            courses = courses.Where(c => c.Status == status);

        return await courses
            .OrderByDescending(c => c.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync(string? query, CourseStatus? status)
    {
        var courses = _context.Courses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            courses = courses.Where(c => c.Title.Contains(query));

        if (status.HasValue)
            courses = courses.Where(c => c.Status == status);

        return await courses.CountAsync();
    }

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }
}