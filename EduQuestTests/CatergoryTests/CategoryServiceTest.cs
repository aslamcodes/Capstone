using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.CourseCategories;
using Moq;

namespace EduQuestTests.CatergoryTests;

[TestFixture]
public class CategoryServiceTests
{
    private Mock<IRepository<int, CourseCategory>> _mockCategoryRepo;
    private IMapper _mapper;
    private CategoryService _categoryService;

    [SetUp]
    public void SetUp()
    {
        _mockCategoryRepo = new Mock<IRepository<int, CourseCategory>>();

        var config =
            new MapperConfiguration(cfg => { cfg.CreateMap<CourseCategory, CourseCategoryDto>().ReverseMap(); });

        _mapper = config.CreateMapper();

        _categoryService = new CategoryService(_mockCategoryRepo.Object, _mapper);
    }

    [Test]
    public async Task Add_WhenCalled_AddsCategory()
    {
        // Arrange
        var categoryDto = new CourseCategoryDto { Id = 1, Name = "Test Category" };
        var category = _mapper.Map<CourseCategory>(categoryDto);

        _mockCategoryRepo.Setup(repo => repo.Add(It.IsAny<CourseCategory>())).ReturnsAsync(category);

        // Act
        var result = await _categoryService.Add(categoryDto);

        // Assert
        Assert.AreEqual(categoryDto.Id, result.Id);
        Assert.AreEqual(categoryDto.Name, result.Name);
        _mockCategoryRepo.Verify(repo => repo.Add(It.IsAny<CourseCategory>()), Times.Once);
    }

    [Test]
    public async Task GetById_WhenCalled_ReturnsCategory()
    {
        // Arrange
        var categoryId = 1;
        var category = new CourseCategory { Id = categoryId, Name = "Test Category" };

        _mockCategoryRepo.Setup(repo => repo.GetByKey(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _categoryService.GetById(categoryId);

        // Assert
        Assert.AreEqual(categoryId, result.Id);
        Assert.AreEqual(category.Name, result.Name);
        _mockCategoryRepo.Verify(repo => repo.GetByKey(categoryId), Times.Once);
    }

    [Test]
    public async Task Update_WhenCalled_UpdatesCategory()
    {
        // Arrange
        var categoryDto = new CourseCategoryDto { Id = 1, Name = "Updated Category" };
        var category = _mapper.Map<CourseCategory>(categoryDto);

        _mockCategoryRepo.Setup(repo => repo.Update(It.IsAny<CourseCategory>())).ReturnsAsync(category);

        // Act
        var result = await _categoryService.Update(categoryDto);

        // Assert
        Assert.AreEqual(categoryDto.Id, result.Id);
        Assert.AreEqual(categoryDto.Name, result.Name);
        _mockCategoryRepo.Verify(repo => repo.Update(It.IsAny<CourseCategory>()), Times.Once);
    }

    [Test]
    public async Task Delete_WhenCalled_DeletesCategory()
    {
        // Arrange
        var categoryId = 1;

        _mockCategoryRepo.Setup(repo => repo.Delete(categoryId)).ReturnsAsync(new CourseCategory() { Id = 1 });

        // Act
        await _categoryService.DeleteById(categoryId);

        // Assert
        _mockCategoryRepo.Verify(repo => repo.Delete(categoryId), Times.Once);
    }
}