using _2_Domain.Entities;
using _2_Domain.Enums;
using _2_Domain.Interfaces;
using _3_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace _3_Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId)
            .OrderBy(l => l.Order)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<bool> ExistsWithOrderAsync(Guid courseId, int order)
    {
        return await _context.Lessons
            .AnyAsync(l =>
                l.CourseId == courseId &&
                l.Order == order &&
                !l.IsDeleted);
    }


    public async Task AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }
}