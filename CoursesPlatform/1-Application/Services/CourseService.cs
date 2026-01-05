using _1_Application.DTOs;
using _1_Application.Interfaces;
using _2_Domain.Entities;
using _2_Domain.Enums;
using _2_Domain.Interfaces;
using ClosedXML.Excel;
using OfficeOpenXml;
using LicenseContext = System.ComponentModel.LicenseContext;

namespace _1_Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILessonRepository _lessonRepository;

    public CourseService(
        ICourseRepository courseRepository,
        ILessonRepository lessonRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
    }

    public async Task<Course?> CreateAsync(string title)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = title,
            Status = CourseStatus.Draft,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _courseRepository.AddAsync(course);
        return course;
    }

    public async Task<bool> PublishAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) return false;

        var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);
        if (!lessons.Any()) return false;

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        return true;
    }

    public async Task<bool> UnpublishAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) return false;

        course.Status = CourseStatus.Draft;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) return false;

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        return true;
    }

    public async Task<CourseSummaryDto?> GetSummaryAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) return null;

        return new CourseSummaryDto
        {
            Id = course.Id,
            Title = course.Title,
            TotalLessons = course.Lessons.Count,
            LastModifiedAt = course.UpdatedAt
        };
    }
    
    public async Task<IEnumerable<Course>> SearchAsync(string? query, CourseStatus? status, int page, int pageSize)
    {
        return await _courseRepository.SearchAsync(query, status, page, pageSize);
    }
    
    public async Task<ImportCoursesResultDto> ImportFromExcelAsync(Stream fileStream)
    {
        var result = new ImportCoursesResultDto();

        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheets.First();

        var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header

        foreach (var row in rows)
        {
            result.TotalRows++;

            var title = row.Cell(1).GetString().Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                result.Skipped++;
                continue;
            }

            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = title,
                Status = CourseStatus.Draft,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _courseRepository.AddAsync(course);
            result.Imported++;
        }

        return result;
    }

}