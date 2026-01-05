using _1_Application.Services;
using _2_Domain.Entities;
using _2_Domain.Enums;
using CoursesPlatform.Tests.Domain.Fakes;

namespace CoursesPlatform.Tests.Domain;

public class LessonServiceTests
{
    [Fact]
    public async Task CreateLesson_WithUniqueOrder_ShouldSucceed()
    {
        var courseRepo = new FakeCourseRepository();
        var lessonRepo = new FakeLessonRepository();
        var service = new LessonService(lessonRepo, courseRepo);

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Course",
            Status = CourseStatus.Draft,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await courseRepo.AddAsync(course);

        var lesson = await service.CreateAsync(course.Id, "Lesson", 1);

        Assert.NotNull(lesson);
    }


    [Fact]
    public async Task CreateLesson_WithDuplicateOrder_ShouldFail()
    {
        var courseRepo = new FakeCourseRepository();
        var lessonRepo = new FakeLessonRepository();
        var service = new LessonService(lessonRepo, courseRepo);

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Course",
            Status = CourseStatus.Draft,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await courseRepo.AddAsync(course);

        await lessonRepo.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            CourseId = course.Id,
            Title = "Lesson 1",
            Order = 1
        });

        var lesson = await service.CreateAsync(course.Id, "Lesson 2", 1);

        Assert.Null(lesson);
    }
}