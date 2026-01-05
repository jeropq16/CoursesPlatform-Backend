using _1_Application.DTOs;
using _2_Domain.Entities;
using _2_Domain.Enums;

namespace _1_Application.Interfaces;

public interface ICourseService
{
    Task<Course?> CreateAsync(string title);
    Task<bool> PublishAsync(Guid courseId);
    Task<bool> UnpublishAsync(Guid courseId);
    Task<bool> SoftDeleteAsync(Guid courseId);
    Task<CourseSummaryDto?> GetSummaryAsync(Guid courseId);
    Task<IEnumerable<Course>> SearchAsync(string? query, CourseStatus? status, int page, int pageSize);
    Task<ImportCoursesResultDto> ImportFromExcelAsync(Stream fileStream);

}