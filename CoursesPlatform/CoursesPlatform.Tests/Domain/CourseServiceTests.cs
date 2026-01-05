using _1_Application.Services;
using _2_Domain.Enums;
using CoursesPlatform.Tests.Domain.Fakes;

namespace CoursesPlatform.Tests.Domain;

public class CourseServiceTests
{
    [Fact]
    public async Task PublishCourse_WithLessons_ShouldSucceed()
    {
        var courseRepo = new FakeCourseRepository();
        var lessonRepo = new FakeLessonRepository();
        var service = new CourseService(courseRepo, lessonRepo);

        var course = await service.CreateAsync("Test Course");

        await lessonRepo.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            CourseId = course!.Id,
            Title = "Lesson 1",
            Order = 1
        });

        var result = await service.PublishAsync(course.Id);

        Assert.True(result);
        Assert.Equal(CourseStatus.Published, course.Status);
    }

    [Fact]
    public async Task PublishCourse_WithoutLessons_ShouldFail()
    {
        var service = new CourseService(
            new FakeCourseRepository(),
            new FakeLessonRepository());

        var course = await service.CreateAsync("Empty Course");

        var result = await service.PublishAsync(course!.Id);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteCourse_ShouldBeSoftDelete()
    {
        var service = new CourseService(
            new FakeCourseRepository(),
            new FakeLessonRepository());

        var course = await service.CreateAsync("Course");

        var result = await service.SoftDeleteAsync(course!.Id);

        Assert.True(result);
        Assert.True(course.IsDeleted);
    }
}