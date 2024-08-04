using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.CourseCategories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EduQuestTests.CatergoryTests;

[TestFixture]
public class CourseCategoryControllerTests
{
    private Mock<ICategoryService> _mockCategoryService;
    private CourseCategoryController _controller;
    private IMapper _mockMapper;

    [SetUp]
    public void SetUp()
    {
        _mockCategoryService = new Mock<ICategoryService>();
        _controller = new CourseCategoryController(_mockCategoryService.Object);

        var config =
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseCategory, CourseCategoryDto>().ReverseMap();
            });
        _mockMapper = config.CreateMapper();

    }

    [Test]
    public async Task GetCourseCategories_ReturnsOkResult_WithListOfCategories()
    {
        // Arrange
        var categories = new List<CourseCategory>
        {
            new CourseCategory { Id = 1, Name = "Category 1" },
            new CourseCategory() { Id = 2, Name = "Category 2" }
        };
        _mockCategoryService.Setup(service => service.GetAll()).ReturnsAsync(categories.ConvertAll(_mockMapper.Map<CourseCategoryDto>));

        // Act
        var result = await _controller.GetCourseCategories();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsInstanceOf<List<CourseCategoryDto>>(okResult.Value);
    }

    [Test]
    public async Task GetCourseCategories_ReturnsStatusCode500_OnException()
    {
        // Arrange
        _mockCategoryService.Setup(service => service.GetAll()).ThrowsAsync(new System.Exception());

        // Act
        var result = await _controller.GetCourseCategories();

        // Assert
        Assert.IsInstanceOf<StatusCodeResult>(result.Result);
        var statusCodeResult = result.Result as StatusCodeResult;
        Assert.AreEqual(500, statusCodeResult.StatusCode);
    }
}